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
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

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
                    .AddCustomSwagger(Configuration)
                    .AddCoutomAuthentication(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "DncAdmin Api");
            });

            app.UseAuthentication();

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

                // 显示项目xml文档
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtAuthenticationSettings>();

                var security = new Dictionary<string, IEnumerable<string>>
                { { jwtSettings.Issuer, new string[] { } }};
                options.AddSecurityRequirement(security);

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

        public static IServiceCollection AddCoutomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtAuthenticationSettings>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    SaveSigninToken = true,//保存token,后台验证token是否生效(重要)
                    ValidateIssuer = true,//是否验证Issuer
                    ValidateAudience = true,//是否验证Audience
                    ValidateLifetime = true,//是否验证失效时间
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    ValidAudience = jwtSettings.Audience,//Audience
                    ValidIssuer = jwtSettings.Issuer,//Issuer，这两项和前面签发jwt的设置一致
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                    AudienceValidator = (IEnumerable<string> audiences, SecurityToken securityToken,
                    TokenValidationParameters validationParameters) =>
                    {
                        bool audienceValidator = true;
                        return audienceValidator;
                    }
                };
            });

            return services;
        }
    }
}
