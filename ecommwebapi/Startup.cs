using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ecommwebapi.Data;
using Ecommwebapi.Helpers;
using Ecommwebapi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.OpenApi.Models;

namespace Ecommwebapi
{
    public class Startup
    {
        private string corsPolicyAllowAll = "AllowAllHeaders";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration
        {
            get;
        }

        //private DbConnection _connection;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //This ideally should be changed for more specific permissions
            services.AddCors(options =>
            {
                options.AddPolicy(corsPolicyAllowAll,
                    builder =>
                {
                    builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                });
            });

            services.AddControllers();

            //configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            //configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddTransient<UserSeeder>();

            //use in memory db context - but using sqlite in memory should be closer to real databases
            // services.AddDbContext<IUserContext, UserContext>(
            //     options => options.UseInMemoryDatabase(databaseName: "estore-test")
            //     );

            services.AddDbContext<IUserContext, UserContext>(cfg =>
                {
                    //cfg.UseSqlite("Filename=:memory:");
                    cfg.UseSqlite(CreateInMemoryDatabase());
                    //cfg.UseSqlite("DataSource=file::memory:");
                }, ServiceLifetime.Singleton, ServiceLifetime.Singleton
            );
            //_connection = RelationalOptionsExtension.Extract(ContextOptions).Connection;

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IDataService, MockDataService>();

            //configure DI for application services
            services.AddScoped<IUserService, UserService>();
            //User singleton because otherwise scoped would reinit our mock data per each request.
            //Would not be a problem for actual DB repo.
            //services.AddSingleton<IUserRepo, MockUserRepo>();

            // Register the Swagger generator and define a Swagger document 
            // for Northwind service 
            services.AddSwaggerGen(options => 
            { 
                options.SwaggerDoc(name: "v1", info: 
                    new OpenApiInfo { Title = "Northwind Service API", Version = "v1" }); 
            });

        }

        //https://github.com/dotnet/efcore/issues/4922
        //https://github.com/dotnet/efcore/issues/7924
        //https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/sqlite#writing-tests
        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }

        //public void Dispose() => _connection.Dispose();

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(corsPolicyAllowAll);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", 
                    "ECommWebAPI Version 1");
                options.SupportedSubmitMethods(new[] 
                    { SubmitMethod.Get, SubmitMethod.Post, 
                      SubmitMethod.Put, SubmitMethod.Delete });
            });
        }
    }
}
