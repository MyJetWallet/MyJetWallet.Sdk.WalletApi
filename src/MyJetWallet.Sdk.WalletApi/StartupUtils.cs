using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyJetWallet.Sdk.Authorization.Http;
using MyJetWallet.Sdk.Authorization.NoSql;
using MyJetWallet.Sdk.NoSql;
using MyJetWallet.Sdk.RestApiTrace;
using MyJetWallet.Sdk.WalletApi.Common;
using MyJetWallet.Sdk.WalletApi.Middleware;
using Prometheus;
using SimpleTrading.BaseMetrics;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MyJetWallet.Sdk.Service;
using MyJetWallet.Sdk.Service.LivenessProbs;

namespace MyJetWallet.Sdk.WalletApi
{
    public static class StartupUtils
    {
        /// <summary>
        /// Setup swagger ui ba
        /// </summary>
        /// <param name="services"></param>
        public static void SetupSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Bearer Token", 
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "Bearer"
                });
                
                option.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
            
            // services.AddSwaggerDocument(o =>
            // {
            //     o.Title = "MyJetWallet API";
            //     o.SchemaSettings.GenerateEnumMappingDescription = true;
            //
            //     o.AddSecurity("Bearer", Enumerable.Empty<string>(),
            //         new OpenApiSecurityScheme
            //         {
            //             Type = OpenApiSecuritySchemeType.ApiKey,
            //             Description = "Bearer Token",
            //             In = OpenApiSecurityApiKeyLocation.Header,
            //             Name = "Authorization"
            //         });
            // });
        }

        /// <summary>
        /// Headers settings
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigurateHeaders(this IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        public static void SetupWalletServices(IServiceCollection services, string sessionEncryptionApiKeyId)
        {
            services.SetupSwaggerDocumentation();
            services.ConfigurateHeaders();
            services.AddControllers(options =>
            {

            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new MyDoubleConverter());
            });

            services
                .AddAuthentication(o => { o.DefaultScheme = "Bearer"; })
                .AddScheme<MyAuthenticationOptions, RootSessionAuthHandler>("Bearer", o =>
                {
                    o.SessionEncryptionApiKeyId = sessionEncryptionApiKeyId;
                });

            services
                .AddAuthorization(o => o.SetupWalletApiPolicy());
        }
        
        public static void SetupSimpleServices(IServiceCollection services, string sessionEncryptionApiKeyId)
        {
            services.SetupSwaggerDocumentation();
            services.ConfigurateHeaders();
            services.AddControllers(options =>
            {

            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new MyDoubleConverter());
            });

            services
                .AddAuthentication(o => { o.DefaultScheme = "Bearer"; })
                .AddScheme<MyAuthenticationOptions, SimpleAuthHandler>("Bearer", o =>
                {
                    o.SessionEncryptionApiKeyId = sessionEncryptionApiKeyId;
                });

            services
                .AddAuthorization(o => o.SetupWalletApiPolicy());
        }

        public static void SetupWalletApplication(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            bool enableApiTrace,
            string swaggerOffsetName)
        {
            if (env.IsDevelopment())
            {
                TokensManager.TokensManager.DebugMode = true;
                RootSessionAuthHandler.IsDevelopmentEnvironment = true;
            }

            app.UseForwardedHeaders();

            if (enableApiTrace)
            {
                ApiTraceMiddleware.ContextHandlerCallback = (context, item) =>
                {
                    if (context.Response.Headers.TryGetValue(ExceptionLogMiddleware.RejectCodeHeader,
                            out var rejectCode))
                    {
                        item.RejectCode = rejectCode;
                    }
                    else
                    {
                        item.RejectCode = "OK";
                    }
                };
                
                app.UseMiddleware<ApiTraceMiddleware>();
                Console.WriteLine("API Trace is Enabled");
            }
            else
            {
                Console.WriteLine("API Trace is Disabled");
            }

            app.BindMetricsMiddleware();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseMetricServer();

            app.BindDependenciesTree(Assembly.GetExecutingAssembly());
            app.BindIsAliveEndpoint();
            app.UseMiddleware<IsAlive2Middleware>();

            app.UseSwagger();
            app.UseSwaggerUI(settings =>
            {
                settings.SwaggerEndpoint($"/swagger/{swaggerOffsetName}/swagger.json", "v1");
                settings.RoutePrefix = $"/swagger/{swaggerOffsetName}";
                settings.DocumentTitle = $"{swaggerOffsetName.ToUpper()} API";
            });
            
            // app.UseOpenApi(settings =>
            // {
            //     settings.Path = $"/swagger/{swaggerOffsetName}/swagger.json";
            // });
            //
            // app.UseSwaggerUi(settings =>
            // {
            //     settings.EnableTryItOut = true;
            //     settings.Path = $"/swagger/{swaggerOffsetName}";
            //     settings.DocumentPath = $"/swagger/{swaggerOffsetName}/swagger.json";
            //     settings.DocumentTitle = $"{swaggerOffsetName.ToUpper()} API";
            // });

            app.UseMiddleware<ExceptionLogMiddleware>();
            app.UseMiddleware<DebugMiddleware>();
            app.UseMiddleware<ApiAccessMiddleware>();
            
            app.UseAuthentication();
            app.UseAuthorization();
        }

        public static void RegisterAuthServices(ContainerBuilder builder, 
            string readerHostPort,
            ILoggerFactory loggerFactory)
        {
            var authNoSql = builder.CreateNoSqlClient(readerHostPort, loggerFactory, "AuthNoSql");
            builder.RegisterMyNoSqlReader<ShortRootSessionNoSqlEntity>(authNoSql, ShortRootSessionNoSqlEntity.TableName);
        }
    }
}