using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.Extensions.Configuration;
using Ecommwebapi.Data;
using System.Linq;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ecommwebapi.Integration.Tests
{
    public class IntegrationTest
    {
        private readonly ITestOutputHelper output;

        public IntegrationTest(ITestOutputHelper output)
        {
            //Can use this output to write to console
            this.output = output;
        }

        //Essentially just an example. Does not even use our actual webapi
        //project for test
        [Fact]
        public async Task BasicIntegrationTest()
        {
            // Arrange
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    // Add TestServer
                    webHost.UseTestServer();

                    // Specify the environment
                    webHost.UseEnvironment("Test");

                    webHost.Configure(app => app.Run(
                        async ctx => await ctx.Response.WriteAsync("Hello World!"))
                    );
                });

            // Create and start up the host
            var host = await hostBuilder.StartAsync();

            // Create an HttpClient which is setup for the test host
            var client = host.GetTestClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Be("Hello World!");
        }

        public static string GetProjectPath(Type startupClass)
        {
            var assembly = startupClass.GetTypeInfo().Assembly;
            var projectName = assembly.GetName().Name;
            var applicationBasePath = AppContext.BaseDirectory;
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                directoryInfo = directoryInfo.Parent;

                var projectDirectoryInfo = new DirectoryInfo(directoryInfo.FullName);
                if (projectDirectoryInfo.Exists)
                {
                    var projectFileInfo = new FileInfo(Path.Combine(projectDirectoryInfo.FullName, projectName, $"{projectName}.csproj"));
                    if (projectFileInfo.Exists)
                    {
                        return Path.Combine(projectDirectoryInfo.FullName, projectName);
                    }
                }
            } while (directoryInfo.Parent != null);

            return "";
        }

        [Fact]
        public async Task BasicEndpointTest()
        {
            //Arrange

            var config = new ConfigurationBuilder().
                AddJsonFile("appsettings.json")
                .Build();

            var dataSource = Path.Combine(GetProjectPath(typeof(IntegrationTest)), "ecommTest.db");
            //var dataSource = @"c:\study\TestProj\ecommwebapi\Ecommwebapi\ecomm.db";
            output.WriteLine("db path: " + dataSource);

            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    //Add TestServer
                    webHost.UseTestServer();
                    webHost.UseConfiguration(config);
                    webHost.UseStartup<Startup>();

                    // configure the services after the startup has been called.
                    webHost.ConfigureTestServices(services =>
                    {
                        //https://stackoverflow.com/questions/58375527/override-ef-core-dbcontext-in-asp-net-core-webapplicationfactory
                        services.RemoveAll(typeof(DbContextOptions<DataContext>));
                        
                        dataSource = @"c:\study\TestProj\ecommwebapi\Ecommwebapi\ecomm.db";
                        services.AddDbContext<IDataContext, DataContext>(cfg =>
                        {
                            cfg.UseSqlite($"Data Source={dataSource};");
                            cfg.EnableSensitiveDataLogging(true);
                        }
                        );
                    });
                });

            //Create and start the host
            var host = await hostBuilder.StartAsync();

            //Create an HttpClient which is setup for the test host
            var client = host.GetTestClient();

            //Act
            var response = await client.GetAsync("api/test");

            //Assert
            var responseString = await response.Content.ReadAsStringAsync();
            output.WriteLine(responseString);
            //responseString.Length.Should().BeGreaterThan(100);
            //responseString.Should().Be("This is a test response");
            responseString.Should().Contain("103");
        }

    }
}
