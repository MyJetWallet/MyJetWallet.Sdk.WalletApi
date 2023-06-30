using System;
using System.Collections.Concurrent;
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
using MyJetWallet.Sdk.Service.Tools;
using MyJetWallet.Sdk.WalletApi.Contracts;
using MyJetWallet.Sdk.WalletApi.Contracts.NoSql;
using MyNoSqlServer.Abstractions;
using Newtonsoft.Json;

// ReSharper disable UnusedMember.Global

namespace MyJetWallet.Sdk.WalletApi.Middleware
{
    public class ApiAccessMiddleware
    {
        public const string RejectCodeHeader = "reject-code";

        private readonly RequestDelegate _next;
        private readonly ILogger<ApiAccessMiddleware> _logger;
        private readonly LocalizationManager _localizationManager;

        private readonly ConcurrentDictionary<(string path, string country), List<DateTime>> _requestsByCountry = new();
        private readonly ConcurrentDictionary<string, List<DateTime>> _requestsByPath = new();

        private readonly IMyNoSqlServerDataReader<ApiAccessSettingsNoSqlEntity> _settingsReader;
        private readonly IMyNoSqlServerDataReader<RestrictedCountriesNoSqlEntity> _restrictedCountriesReader;
        private readonly IMyNoSqlServerDataReader<RestrictedMethodsNoSqlEntity> _restrictedMethodsReader;

        public ApiAccessMiddleware(RequestDelegate next, ILogger<ApiAccessMiddleware> logger,
            LocalizationManager localizationManager,
            IMyNoSqlServerDataReader<ApiAccessSettingsNoSqlEntity> settingsReader,
            IMyNoSqlServerDataReader<RestrictedCountriesNoSqlEntity> restrictedCountriesReader,
            IMyNoSqlServerDataReader<RestrictedMethodsNoSqlEntity> restrictedMethodsReader)
        {
            _next = next;
            _logger = logger;
            _localizationManager = localizationManager;
            _settingsReader = settingsReader;
            _restrictedCountriesReader = restrictedCountriesReader;
            _restrictedMethodsReader = restrictedMethodsReader;

            var timer = new MyTaskTimer(typeof(ApiAccessMiddleware), TimeSpan.FromMinutes(5), logger, DoTime);
            timer.Start();
        }

        private async Task DoTime()
        {
            foreach (var (method, dtList) in _requestsByCountry)
            {
                var list = dtList.Where(t => t < DateTime.UtcNow.AddDays(-1)).ToList();
                _requestsByCountry[method] = list;
            }

            foreach (var (method, dtList) in _requestsByPath)
            {
                var list = dtList.Where(t => t < DateTime.UtcNow.AddDays(-1)).ToList();
                _requestsByPath[method] = list;
            }
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var settings = _settingsReader.Get(ApiAccessSettingsNoSqlEntity.GeneratePartitionKey(),
                    ApiAccessSettingsNoSqlEntity.GenerateRowKey());
                var country = context.GetGeolocationByIp();
                var restrictedCountry = _restrictedCountriesReader.Get(
                    RestrictedCountriesNoSqlEntity.GeneratePartitionKey(),
                    RestrictedCountriesNoSqlEntity.GenerateRowKey(country));
                if (restrictedCountry != null)
                    throw new WalletApiErrorException(ApiResponseCodes.ServiceUnavailable);

                var path = context.Request.Path.Value;
                var restrictedMethod = _restrictedMethodsReader.Get(RestrictedMethodsNoSqlEntity.GeneratePartitionKey(),
                    RestrictedMethodsNoSqlEntity.GenerateRowKey(path));
                if (restrictedMethod != null)
                {
                    _requestsByCountry.TryGetValue((path, country), out var dtList);
                    dtList ??= new List<DateTime>();
                    dtList.Add(DateTime.UtcNow);
                    _requestsByCountry[(path, country)] = dtList;

                    _requestsByPath.TryGetValue(path, out var dtList2);
                    dtList2 ??= new List<DateTime>();
                    dtList2.Add(DateTime.UtcNow);
                    _requestsByPath[path] = dtList2;

                    if (dtList.Count(t => t < DateTime.UtcNow.AddSeconds(-5)) > restrictedMethod.Attempts5Sec)
                        throw new WalletApiErrorException(ApiResponseCodes.ServiceUnavailable);

                    if (dtList.Count(t => t < DateTime.UtcNow.AddMinutes(-1)) > restrictedMethod.Attempts1Min)
                        throw new WalletApiErrorException(ApiResponseCodes.ServiceUnavailable);

                    if (dtList.Count(t => t < DateTime.UtcNow.AddHours(-1)) > restrictedMethod.Attempts1Hour)
                        throw new WalletApiErrorException(ApiResponseCodes.ServiceUnavailable);

                    if (dtList.Count(t => t < DateTime.UtcNow.AddDays(-1)) > restrictedMethod.Attempts1Day)
                        throw new WalletApiErrorException(ApiResponseCodes.ServiceUnavailable);

                    if (settings.EnableBlocksWithoutCountries)
                    {
                        if (dtList2.Count(t => t < DateTime.UtcNow.AddSeconds(-5)) > restrictedMethod.Attempts5Sec)
                            throw new WalletApiErrorException(ApiResponseCodes.ServiceUnavailable);

                        if (dtList2.Count(t => t < DateTime.UtcNow.AddMinutes(-1)) > restrictedMethod.Attempts1Min)
                            throw new WalletApiErrorException(ApiResponseCodes.ServiceUnavailable);

                        if (dtList2.Count(t => t < DateTime.UtcNow.AddHours(-1)) > restrictedMethod.Attempts1Hour)
                            throw new WalletApiErrorException(ApiResponseCodes.ServiceUnavailable);

                        if (dtList2.Count(t => t < DateTime.UtcNow.AddDays(-1)) > restrictedMethod.Attempts1Day)
                            throw new WalletApiErrorException(ApiResponseCodes.ServiceUnavailable);
                    }
                }

                await _next(context);
            }
            catch (WalletApiErrorException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}