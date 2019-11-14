/*
 * CASN API
 *
 * This is a test CASN API
 *
 * OpenAPI spec version: 1.0.0
 * Contact: katie@clinicaccess.org
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.IO;
using CASNApp.Core.Entities;
using CASNApp.API.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CASNApp.API
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnv;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env"></param>
        /// <param name="configuration"></param>
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            _hostingEnv = env;
            _configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services
                //.AddDbContextPool<casn_appContext>(options =>
                //{
                //    options.UseMySql(_configuration["DbConnectionString"], mysqlOptions =>
                //    {
                //        mysqlOptions.ServerVersion(new Version(5, 7, 22), ServerType.MySql)
                //            .EnableRetryOnFailure(2);
                //    });
                //})
                .AddDbContext<casn_appContext>(options =>
                    {
                        options.UseSqlServer(_configuration[Core.Constants.DbConnectionString], sqlOptions =>
                            {
                                sqlOptions
                                    .EnableRetryOnFailure(int.Parse(_configuration[Core.Constants.DBRetryCount]));
                            });
                    }, ServiceLifetime.Scoped, ServiceLifetime.Scoped);

            if (_hostingEnv.IsDevelopment())
            {
                services.AddCors();
            }

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services
                .AddMvc()
                .AddJsonOptions(opts =>
                {
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opts.SerializerSettings.Converters.Add(new StringEnumConverter
                    {
                        NamingStrategy = new CamelCaseNamingStrategy(),
                    });
                });

            var dispatchersRole = _configuration[Constants.DispatchersRoleName];
            var driversRole = _configuration[Constants.DriversRoleName];

            if (string.IsNullOrWhiteSpace(dispatchersRole))
            {
                throw new Exception($"{nameof(Constants.DispatchersRoleName)} is not configured.");
            }

            if (string.IsNullOrWhiteSpace(driversRole))
            {
                throw new Exception($"{nameof(Constants.DriversRoleName)} is not configured.");
            }

            var isDispatcherPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireRole(dispatchersRole)
                .Build();

            var isDriverPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireRole(driversRole)
                .Build();

            var isDispatcherOrDriverPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireAssertion(ctx => 
                    ctx.User.IsInRole(dispatchersRole) ||
                    ctx.User.IsInRole(driversRole))
                .Build();

            services
                .AddAuthorization(options =>
                {
                    options.AddPolicy(Constants.IsDispatcherPolicy, isDispatcherPolicy);
                    options.AddPolicy(Constants.IsDriverPolicy, isDriverPolicy);
                    options.AddPolicy(Constants.IsDispatcherOrDriverPolicy, isDispatcherOrDriverPolicy);
                })
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    options.Authority = _configuration[Constants.JwtBearerAuthority];
                    options.Audience = _configuration[Constants.JwtBearerAudience];
                    options.RequireHttpsMetadata = Constants.JwtBearerRequireHttpsMetadata;
                    options.TokenValidationParameters.RoleClaimType = Constants.JwtBearerRoleClaimType;
                });

            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("1.0.0", new Info
                    {
                        Version = "1.0.0",
                        Title = "CASN API",
                        Description = "CASN API (ASP.NET Core 2.1)",
                        Contact = new Contact()
                        {
                           Name = "CASN App Contributors",
                           Url = "https://github.com/d-m-wilson/casn-app",
                           Email = "katie@clinicaccess.org"
                        },
                        TermsOfService = ""
                    });
                    c.CustomSchemaIds(type => type.FriendlyId(true));
                    c.DescribeAllEnumsAsStrings();
                    c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{_hostingEnv.ApplicationName}.xml");
                    // Sets the basePath property in the Swagger document generated
                    c.DocumentFilter<BasePathFilter>("/api");

                    // Include DataAnnotation attributes on Controller Action parameters as Swagger validation rules (e.g required, pattern, ..)
                    // Use [ValidateModelState] on Actions to actually validate it in C# as well!
                    c.OperationFilter<GeneratePathParamsValidationFilter>();

                    c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = "header",
                        Type = "apiKey"
                    });

                    c.AddSecurityRequirement(new System.Collections.Generic.Dictionary<string, System.Collections.Generic.IEnumerable<string>> { { "Bearer", new string[] { } } });
                });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();

            if (_hostingEnv.IsDevelopment())
            {
                app.UseCors(builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                    builder.SetPreflightMaxAge(TimeSpan.FromMinutes(5));
                });
            }

            app
                .UseAuthentication()
                .UseMvc()
                .UseDefaultFiles()
                .UseStaticFiles()
                .UseSwagger(c =>
                {
                    c.RouteTemplate = "swagger/{documentName}/openapi.json";
                })
                .UseSwaggerUI(c =>
                {
                    //TODO: Either use the SwaggerGen generated Swagger contract (generated from C# classes)
                    c.SwaggerEndpoint("/swagger/1.0.0/openapi.json", "CASN API");

                    //TODO: Or alternatively use the original Swagger contract that's included in the static files
                    // c.SwaggerEndpoint("/openapi-original.json", "CASN API Original");
                });

            if (_hostingEnv.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //TODO: Enable production exception handling (https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling)
                // app.UseExceptionHandler("/Home/Error");
            }
        }
    }
}
