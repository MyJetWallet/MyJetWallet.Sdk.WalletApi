﻿using System;
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
using MyJetWallet.Sdk.WalletApi.Swagger;

namespace MyJetWallet.Sdk.WalletApi
{
    public static class StartupUtils
    {
        /// <summary>
        /// Setup swagger ui ba
        /// </summary>
        /// <param name="services"></param>
        /// <param name="swaggerOffsetName"></param>
        public static void SetupSwaggerDocumentation(this IServiceCollection services, string swaggerOffsetName)
        {
            var baseUrl = Environment.GetEnvironmentVariable("SWAGGER_BASE_URL");
            if (string.IsNullOrEmpty(baseUrl))
                throw new Exception("SWAGGER_BASE_URL environment variable not set");
            
            services.AddSwaggerGen(option =>
            {
                option.SwaggerGeneratorOptions.Servers = new List<OpenApiServer>() { new() { Url = baseUrl },};
                option.SwaggerDoc(swaggerOffsetName, new OpenApiInfo()
                {
                    Title = $"{swaggerOffsetName.ToUpper()} API",
                    Version = "1.0.0",
                    Description = $"{swaggerOffsetName.ToUpper()} API",
                });
                option.SchemaFilter<EnumSchemaFilter>();

                
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

        }

        /// <summary>
        /// Headers settings
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureHeaders(this IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        public static void SetupWalletServices(IServiceCollection services, string sessionEncryptionApiKeyId, string swaggerOffsetName)
        {
            services.SetupSwaggerDocumentation(swaggerOffsetName);
            services.ConfigureHeaders();
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
        
        public static void SetupSimpleServices(IServiceCollection services, string sessionEncryptionApiKeyId, string swaggerOffsetName)
        {
            services.SetupSwaggerDocumentation(swaggerOffsetName);
            services.ConfigureHeaders();
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
            string swaggerOffsetName,
            bool useApiAccessMiddleware = true,
            bool useExceptionLogMiddleware = true,
            bool useAuthentication = true)
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

            app.UseSwagger(options =>
            {
                options.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(settings =>
            {
                settings.SwaggerEndpoint($"swagger.json", "v1");
                settings.RoutePrefix = $"swagger/{swaggerOffsetName}";
                settings.DocumentTitle = $"{swaggerOffsetName.ToUpper()} API";
            });
            
            // app.UseSwagger();
            // app.UseSwaggerUI(settings =>
            // {
            //     settings.SwaggerEndpoint($"/swagger/{swaggerOffsetName}/swagger.json", "v1");
            //     settings.RoutePrefix = $"/swagger/{swaggerOffsetName}";
            //     settings.DocumentTitle = $"{swaggerOffsetName.ToUpper()} API";
            // });
            
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

            if (useExceptionLogMiddleware)
            {
                app.UseMiddleware<ExceptionLogMiddleware>();
            }
            
            app.UseMiddleware<DebugMiddleware>();

            if (useApiAccessMiddleware)
            {
                app.UseMiddleware<ApiAccessMiddleware>();
            }
            
            if(useAuthentication)
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }
        }
    }
}