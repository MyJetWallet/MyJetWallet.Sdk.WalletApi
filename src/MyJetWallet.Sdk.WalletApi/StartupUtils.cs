using System;
using System.Linq;
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
using NSwag;
using Prometheus;
using SimpleTrading.BaseMetrics;
using SimpleTrading.ServiceStatusReporterConnector;
using SimpleTrading.TokensManager;
using Microsoft.Extensions.Logging;

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
            services.AddSwaggerDocument(o =>
            {
                o.Title = "MyJetWallet API";
                o.GenerateEnumMappingDescription = true;

                o.AddSecurity("Bearer", Enumerable.Empty<string>(),
                    new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Description = "Bearer Token",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Name = "Authorization"
                    });
            });
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
                TokensManager.DebugMode = true;
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

            app.BindServicesTree(Assembly.GetExecutingAssembly());

            app.BindIsAlive();

            app.UseOpenApi(settings =>
            {
                settings.Path = $"/swagger/{swaggerOffsetName}/swagger.json";
            });

            app.UseSwaggerUi3(settings =>
            {
                settings.EnableTryItOut = true;
                settings.Path = $"/swagger/{swaggerOffsetName}";
                settings.DocumentPath = $"/swagger/{swaggerOffsetName}/swagger.json";

            });

            app.UseMiddleware<ExceptionLogMiddleware>();
            app.UseMiddleware<DebugMiddleware>();

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