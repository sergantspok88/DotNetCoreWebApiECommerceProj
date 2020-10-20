using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ecommwebapi.Data;
using Ecommwebapi.Helpers;
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
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Ecommwebapi.Data.Repos;
using Ecommwebapi.Data.Services;

namespace Ecommwebapi
{
    public class Startup
    {
        private string corsPolicyAllowAll = "AllowAllHeaders";

        public IConfiguration Configuration
        {
            get;
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

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

            services.AddScoped<UserSeeder>();
            services.AddScoped<ProductSeeder>();

            var dataSource = Path.Combine(Directory.GetCurrentDirectory(), "ecomm.db");
            //var dataSource = @"c:\study\TestProj\ecommwebapi\Ecommwebapi\ecomm.db";
            services.AddDbContext<IDataContext, DataContext>(cfg =>
                {
                    cfg.UseSqlite($"Data Source={dataSource};");
                    cfg.EnableSensitiveDataLogging(true);
                }
            );

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //services.AddScoped<IProductRepo, MockProductRepo>();
            services.AddScoped<IProductRepo, EFProductRepo>();
            services.AddScoped<ICategoryRepo, EFCategoryRepo>();
            services.AddScoped<ICartItemRepo, EFCartItemRepo>();
            services.AddScoped<IWishlistRepo, EFWishlistRepo>();
            services.AddScoped<IOrderRepo, EFOrderRepo>();
            services.AddScoped<IUserRepo, EFUserRepo>();

            services.AddScoped<IUnitOfWork, EFUnitOfWork>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUserService, UserService>();

            // Register the Swagger generator and define a Swagger document 
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(name: "v1", info:
                    new OpenApiInfo { Title = "ECommWebAPI", Version = "v1" });

                //https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

        }

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
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "ECommWebAPI Version 1");
                options.SupportedSubmitMethods(new[]
                    { SubmitMethod.Get, SubmitMethod.Post,
                      SubmitMethod.Put, SubmitMethod.Delete });
            });
        }
    }
}
