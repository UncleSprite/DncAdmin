using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DncAdmin.Api.Infrastructure;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;
using Swashbuckle.AspNetCore.Swagger;
using DncAdmin.Api.Auth;

namespace DncAdmin.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<JwtAuthenticationSettings>(Configuration.GetSection("JwtSettings"));

            services.AddCustomContext(Configuration)
                    .AddCustomMvc()
                    .AddCustomSwagger(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "DncAdmin Api");
            });

            app.UseMvcWithDefaultRoute();
        }
    }

    public static class CustomExtensionMethods
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                //options.Filters.Add()
            })
            .AddControllersAsServices();

            return services;
        }

        public static IServiceCollection AddCustomContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DncAdminContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionString"], sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });

                options.ConfigureWarnings(warning => warning.Throw(RelationalEventId.QueryClientEvaluationWarning));
            });
            return services;
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Description = "DncAdmin Api Doc",
                    TermsOfService = "None",
                    Title = "DncAdmin Api 接口文档",
                    Version = "V1"
                });

                var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtAuthenticationSettings>();

                // 配置swagger权限
                options.AddSecurityDefinition(jwtSettings.Issuer, new ApiKeyScheme
                {
                    Description = "JWT授权token前面需要加上字段Bearer与一个空格,如Bearer 12345x",
                    Type = "apiKey",
                    In = "header",
                    Name = "Authorization"
                });
            });

            return services;
        }
    }

}
