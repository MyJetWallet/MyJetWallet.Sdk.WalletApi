using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service;
using MyJetWallet.Sdk.WalletApi.Contracts;
using Newtonsoft.Json;

// ReSharper disable UnusedMember.Global

namespace MyJetWallet.Sdk.WalletApi.Middleware
{
    public class ExceptionLogMiddleware
    {
        public const string RejectCodeHeader = "reject-code";
        
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionLogMiddleware> _logger;
        private readonly LocalizationManager _localizationManager;

        public ExceptionLogMiddleware(RequestDelegate next, ILogger<ExceptionLogMiddleware> logger, LocalizationManager localizationManager)
        {
            _next = next;
            _logger = logger;
            _localizationManager = localizationManager;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (WalletApiHttpException ex)
            {
                ex.StatusCode.AddToActivityAsTag("ErrorCode");
                ex.FailActivity();
                _logger.LogInformation(ex,
                    "Receive WalletApiHttpException with status code: {StatusCode}; path: {Path}", ex.StatusCode,
                    context.Request.Path);

                context.Response.StatusCode = (int) ex.StatusCode;
                await context.Response.WriteAsJsonAsync(new {ex.Message});
            }
            catch (WalletApiErrorBlockerException ex)
            {
                var clientId = context.User?.Claims?.FirstOrDefault(e => e.Type == "Client-Id")?.Value;
                ex.Code.AddToActivityAsTag("ErrorCode");
                ex.FailActivity();
                
                _logger.LogInformation(ex, "Receive WalletApiErrorBlockerException with status code: {codeText}; path: {Path}; clientId: '{clientId}'", 
                    ex.Code.ToString(), context.Request.Path, clientId);

                context.Response.StatusCode = (int) HttpStatusCode.OK;
                context.Response.Headers.TryAdd(RejectCodeHeader, ex.Code.ToString());

                //TODO: after refactoring on client side need to remove Response<UnauthorizedData> and use simple Response
                //var response = Response.RejectWithDetails(ex.Code, ex.UnauthorizedData);
                
                var message = await _localizationManager.GetTemplateBody(ex.Code, context, ex.TemplateParams);

                var response = new Response<UnauthorizedData>(ex.Code, message, ex.UnauthorizedData)
                {
                    RejectDetail = ex.UnauthorizedData
                };
                
                await context.Response.WriteAsJsonAsync(response);
            }
            catch (WalletApiErrorException ex)
            {
                ex.Code.AddToActivityAsTag("ErrorCode");
                ex.FailActivity();
                _logger.LogInformation(ex, "Receive WalletApiErrorException with status code: {codeText}; path: {Path}", ex.Code.ToString(), context.Request.Path);

                context.Response.StatusCode = (int) HttpStatusCode.OK;
                context.Response.Headers.TryAdd(RejectCodeHeader, ex.Code.ToString());

                var message = await _localizationManager.GetTemplateBody(ex.Code, context, ex.TemplateParams);
                
                //TODO: Remove
                if (ex.Code == ApiResponseCodes.CardCountryNotSupported)
                {
                    var workAroundMesage = "{\n   \"data\":{\n      \"cardId\":\"fc2f410910684ec1a7886a319b63cadc\",\n      \"status\":1,\n      \"requiredVerification\":0\n   },\n   \"result\":\"CardCountryNotSupported\",\n   \"rejectDetail\":null,\n   \"message\":\"Card Country Not Supported\"\n}";
                    await context.Response.WriteAsync(workAroundMesage);
                }
                //TODO: Remove
                else if (ex.Code == ApiResponseCodes.CardCountryNotSupportedExceptVisa)
                {
                    var workAroundMesage = "{\n   \"data\":{\n      \"cardId\":\"b2588207375b4221b0bddac526093812\",\n      \"status\":1,\n      \"requiredVerification\":0\n   },\n   \"result\":\"CardCountryNotSupportedExceptVisa\",\n   \"rejectDetail\":null,\n   \"message\":\"Card Country Not Supported Except Visa\"\n}";
                    await context.Response.WriteAsync(workAroundMesage);                }
                else
                    await context.Response.WriteAsJsonAsync(new Response(ex.Code, message));
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        // private async Task<string> GetMessage(ApiResponseCodes exCode, HttpContext context)
        // {
        //     var lang = context.GetLang();
        //     var template = await _templateClient.GetTemplateBody(exCode.ToString(), "simple", lang);
        //     return template;
        // }
    }

    public class DebugMiddleware
    {
        private readonly RequestDelegate _next;

        public DebugMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/api/Debug/make-signature" && context.Request.Method == "POST")
            {
                var request = context.Request;

                if (!context.Request.Headers.TryGetValue("private-key", out var key))
                {
                    context.Response.StatusCode = 400;
                    return;
                }

                string bodyStr;
                using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                    Console.WriteLine(bodyStr);

                    var rsa = RSA.Create(2048);

                    rsa.ImportRSAPrivateKey(Convert.FromBase64String(key), out _);

                    var signature = rsa.SignData(Encoding.UTF8.GetBytes(bodyStr), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                    var response = new
                    {
                        Signature = Convert.ToBase64String(signature)
                    };

                    await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)));
                }


                return;
            }

            await _next(context);
        }

    }
}