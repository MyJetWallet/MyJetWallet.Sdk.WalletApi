using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service.Tools;
using MyJetWallet.Sdk.WalletApi.Contracts;
using MyJetWallet.Sdk.WalletApi.Contracts.NoSql;
using MyNoSqlServer.Abstractions;
using SimpleTrading.ClientApi.Utils;

// ReSharper disable UnusedMember.Global

namespace MyJetWallet.Sdk.WalletApi.Middleware
{
    public class ApiAccessMiddleware
    {
        public const string RejectCodeHeader = "reject-code";

        private readonly RequestDelegate _next;
        private readonly ILogger<ApiAccessMiddleware> _logger;

        private readonly ConcurrentDictionary<(string path, string country), List<DateTime>> _requestsByCountry = new();
        private readonly ConcurrentDictionary<string, List<DateTime>> _requestsByPath = new();
        private readonly ConcurrentDictionary<(string path, string ip), List<DateTime>> _requestsByIps = new();

        private readonly ConcurrentDictionary<(string path, string ip), DateTime> _blockedIps = new();
        private readonly ConcurrentDictionary<(string path, string country), DateTime> _blockedCountries = new();

        private readonly IMyNoSqlServerDataReader<ApiAccessSettingsNoSqlEntity> _settingsReader;
        private readonly IMyNoSqlServerDataReader<RestrictedCountriesNoSqlEntity> _restrictedCountriesReader;
        private readonly IMyNoSqlServerDataReader<RestrictedMethodsNoSqlEntity> _restrictedMethodsReader;
        private readonly IMyNoSqlServerDataReader<RestrictedIPsNoSqlEntity> _restrictedIPsReader;

        private const string AllCountriesCode = "ALL COUNTRIES";


        public ApiAccessMiddleware(RequestDelegate next, ILogger<ApiAccessMiddleware> logger,
            IMyNoSqlServerDataReader<ApiAccessSettingsNoSqlEntity> settingsReader,
            IMyNoSqlServerDataReader<RestrictedCountriesNoSqlEntity> restrictedCountriesReader,
            IMyNoSqlServerDataReader<RestrictedMethodsNoSqlEntity> restrictedMethodsReader,
            IMyNoSqlServerDataReader<RestrictedIPsNoSqlEntity> restrictedIPsReader)
        {
            _next = next;
            _logger = logger;
            _settingsReader = settingsReader;
            _restrictedCountriesReader = restrictedCountriesReader;
            _restrictedMethodsReader = restrictedMethodsReader;
            _restrictedIPsReader = restrictedIPsReader;

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

            foreach (var (method, dtList) in _requestsByIps)
            {
                var list = dtList.Where(t => t < DateTime.UtcNow.AddDays(-1)).ToList();
                _requestsByIps[method] = list;
            }

            foreach (var (key, ts) in _blockedCountries)
                if (ts < DateTime.UtcNow)
                    _blockedCountries.TryRemove(key, out _);

            foreach (var (key, ts) in _blockedIps)
                if (ts < DateTime.UtcNow)
                    _blockedIps.TryRemove(key, out _);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var country = context.GetGeolocationByIp();
                var ip = context.GetIp();
                var path = context.Request.Path.Value;

                if (_blockedIps.TryGetValue((path, ip), out var blocker))
                {
                    _logger.LogInformation("Client with ip {ip} try to access to {path} but blocked till {blocker}",
                        ip, path, blocker);
                    throw new WalletApiErrorException(ApiResponseCodes.ServiceUnavailable);
                }

                if (_blockedCountries.TryGetValue((path, country), out var blocker2))
                {
                    _logger.LogInformation(
                        "Client with ip {ip} and country {country} try to access to {path} but blocked till {blocker}",
                        ip, country, path, blocker2);
                    throw new WalletApiErrorException(ApiResponseCodes.ServiceUnavailable);
                }

                if (_blockedCountries.TryGetValue((path, AllCountriesCode), out var blocker3))
                {
                    _logger.LogInformation(
                        "Client try to access to {path} but blocked till {blocker}. All requests from all countries are blocked",
                        path, blocker3);
                    throw new WalletApiErrorException(ApiResponseCodes.ServiceUnavailable);
                }


                var restrictedCountry = _restrictedCountriesReader.Get(
                    RestrictedCountriesNoSqlEntity.GeneratePartitionKey(),
                    RestrictedCountriesNoSqlEntity.GenerateRowKey(country));
                if (restrictedCountry != null)
                    throw new WalletApiErrorException(ApiResponseCodes.ServiceUnavailable);

                var settings = _settingsReader.Get(ApiAccessSettingsNoSqlEntity.GeneratePartitionKey(),
                    ApiAccessSettingsNoSqlEntity.GenerateRowKey());
                var blockerTime = DateTime.UtcNow + (settings?.BlockDuration ?? TimeSpan.FromHours(1));

                var restrictedMethod = _restrictedMethodsReader.Get(RestrictedMethodsNoSqlEntity.GeneratePartitionKey(),
                    RestrictedMethodsNoSqlEntity.GenerateRowKey(path));
                if (restrictedMethod != null)
                {
                    _requestsByCountry.TryGetValue((path, country), out var dtList);
                    dtList ??= new List<DateTime>();

                    _requestsByPath.TryGetValue(path, out var dtList2);
                    dtList2 ??= new List<DateTime>();

                    if (dtList.Count(t => t < DateTime.UtcNow.AddSeconds(-5)) > restrictedMethod.Attempts5Sec)
                        await SetBlockerCountry(country, path, blockerTime, restrictedMethod.Attempts5Sec, "5 sec");

                    if (dtList.Count(t => t < DateTime.UtcNow.AddMinutes(-1)) > restrictedMethod.Attempts1Min)
                        await SetBlockerCountry(country, path, blockerTime, restrictedMethod.Attempts1Min, "1 min");

                    if (dtList.Count(t => t < DateTime.UtcNow.AddHours(-1)) > restrictedMethod.Attempts1Hour)
                        await SetBlockerCountry(country, path, blockerTime, restrictedMethod.Attempts1Hour, "1 hour");

                    if (dtList.Count(t => t < DateTime.UtcNow.AddDays(-1)) > restrictedMethod.Attempts1Day)
                        await SetBlockerCountry(country, path, blockerTime, restrictedMethod.Attempts1Day, "1 day");

                    dtList.Add(DateTime.UtcNow);
                    _requestsByCountry[(path, country)] = dtList;

                    if (settings?.EnableBlocksWithoutCountries ?? false)
                    {
                        if (dtList2.Count(t => t < DateTime.UtcNow.AddSeconds(-5)) > restrictedMethod.Attempts5Sec)
                            await SetBlockerCountry(AllCountriesCode, path, blockerTime, restrictedMethod.Attempts5Sec,
                                "5 sec");
                        if (dtList2.Count(t => t < DateTime.UtcNow.AddMinutes(-1)) > restrictedMethod.Attempts1Min)
                            await SetBlockerCountry(AllCountriesCode, path, blockerTime, restrictedMethod.Attempts1Min,
                                "1 min");
                        if (dtList2.Count(t => t < DateTime.UtcNow.AddHours(-1)) > restrictedMethod.Attempts1Hour)
                            await SetBlockerCountry(AllCountriesCode, path, blockerTime, restrictedMethod.Attempts1Hour,
                                "1 hour");
                        if (dtList2.Count(t => t < DateTime.UtcNow.AddDays(-1)) > restrictedMethod.Attempts1Day)
                            await SetBlockerCountry(AllCountriesCode, path, blockerTime, restrictedMethod.Attempts1Day,
                                "1 day");
                    }

                    dtList2.Add(DateTime.UtcNow);
                    _requestsByPath[path] = dtList2;
                }

                var restrictedIp = _restrictedIPsReader.Get(RestrictedMethodsNoSqlEntity.GeneratePartitionKey(),
                    RestrictedMethodsNoSqlEntity.GenerateRowKey(ip));

                if (restrictedIp != null)
                {
                    _requestsByIps.TryGetValue((path, ip), out var dtList);
                    dtList ??= new List<DateTime>();

                    if (dtList.Count(t => t < DateTime.UtcNow.AddSeconds(-5)) > restrictedIp.Attempts5Sec)
                        await SetBlockerIp(ip, path, blockerTime, restrictedIp.Attempts5Sec, "5 sec");
                    if (dtList.Count(t => t < DateTime.UtcNow.AddMinutes(-1)) > restrictedIp.Attempts1Min)
                        await SetBlockerIp(ip, path, blockerTime, restrictedIp.Attempts1Min, "1 min");
                    if (dtList.Count(t => t < DateTime.UtcNow.AddHours(-1)) > restrictedIp.Attempts1Hour)
                        await SetBlockerIp(ip, path, blockerTime, restrictedIp.Attempts1Hour, "1 hour");
                    if (dtList.Count(t => t < DateTime.UtcNow.AddDays(-1)) > restrictedIp.Attempts1Day)
                        await SetBlockerIp(ip, path, blockerTime, restrictedIp.Attempts1Day, "1 day");

                    dtList.Add(DateTime.UtcNow);
                    _requestsByIps[(path, ip)] = dtList;
                }

                await _next(context);
            }
            catch (WalletApiErrorException)
            {
                throw;
            }
            catch (WalletApiErrorBlockerException)
            {
                throw;
            }
            catch (WalletApiHttpException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task SetBlockerCountry(string country, string path, DateTime blockTime, int attempts,
            string duration)
        {
            _logger.LogError(
                "!!! API BLOCK !!! Client from {country} try to access [{path}]  {attempts} times per {duration} and blocked till {blockTime}",
                country, path, attempts, duration, blockTime);
            _blockedCountries[(path, country)] = blockTime;
            throw new WalletApiErrorException(ApiResponseCodes.ServiceUnavailable);
        }

        private async Task SetBlockerIp(string ip, string path, DateTime blockTime, int attempts, string duration)
        {
            _logger.LogError(
                "!!! API BLOCK !!!  Client with ip {ip} try to access [{path}]  {attempts} times per {duration} and blocked till {blockTime}",
                ip, path, attempts, duration, blockTime);
            _blockedIps[(path, ip)] = blockTime;
            throw new WalletApiErrorException(ApiResponseCodes.ServiceUnavailable);
        }
    }
}