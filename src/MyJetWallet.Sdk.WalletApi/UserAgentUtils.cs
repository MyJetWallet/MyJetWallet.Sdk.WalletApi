using System;
using Microsoft.AspNetCore.Http;
using SimpleTrading.Common.Helpers;

namespace MyJetWallet.Sdk.WalletApi
{
    public static class UserAgentUtils
    {
        private const string UserAgent = "User-Agent";
        private const string Ipcountry = "cf-ipcountry";
        
        public static string GetGeolocationByIp(this HttpContext ctx)
        {
            if (ctx.Request.Headers.ContainsKey(Ipcountry))
                return ctx.Request.Headers[Ipcountry].ToString();
            
            return "";
        }
        
        public static string GetRowUserAgent(this HttpContext ctx)
        {
            if (ctx.Request.Headers.ContainsKey(UserAgent))
                return ctx.Request.Headers[UserAgent].ToString();

            return string.Empty;
        }
        
        public static string GetLang(this HttpContext ctx)
        {
            try
            {
                var userAgent = ctx.GetRowUserAgent();
                var split = userAgent.Split(';');
                var lang = split[5];
                return lang.Length == 2 ? lang : string.Empty;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        
        public static string GetPhoneModel(this HttpContext ctx)
        {
            try
            {
                var userAgent = ctx.GetRowUserAgent();
                var split = userAgent.Split(';');
                var model = split[6];
                return model;
            }
            catch (Exception e)
            {
                return "Unknown device";
            }
        }
        
        public static string GetCountryFull(this HttpContext ctx)
        {
            var country = ctx.GetGeolocationByIp();
            return CountryManager.GetCountryNameByIso2(country);
        }
        
        public static string GetDeviceUid(this HttpContext ctx)
        {
            try
            {
                var userAgent = ctx.GetRowUserAgent();
                var split = userAgent.Split(';');
                var deviceUid = split[7];
                return deviceUid;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
    }
}