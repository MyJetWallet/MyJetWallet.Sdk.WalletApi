using System;
using Microsoft.AspNetCore.Http;
using MyJetWallet.Sdk.WalletApi.Common;
using SimpleTrading.Common.Helpers;

namespace MyJetWallet.Sdk.WalletApi
{
    public static class UserAgentUtils
    {
        // 1.9.3;3791;ios;Size(414.0, 896.0);3.0;en;iPhone XS Max;56147CFB-0000-0000-0000-D4E97E2323BF
        
        private const string OurUserAgent = "X-User-Agent";
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
            if (ctx.Request.Headers.ContainsKey(OurUserAgent))
                return ctx.Request.Headers[OurUserAgent].ToString();
            
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
                var lang = split.Length >= 6 ? split[5] : String.Empty;
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
                var model = split.Length >= 7 ? split[6] : string.Empty;
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
                var deviceUid = split.Length >= 8 ? split[7] : string.Empty;
                return deviceUid;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        
        public static string GetDevicePlatform(this HttpContext ctx)
        {
            try
            {
                var userAgent = ctx.GetRowUserAgent();
                var split = userAgent.Split(';');
                var platform = split.Length >= 3 ? split[2] : string.Empty;
                return platform;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        
        public static string GetClientApplicationVersion(this HttpContext ctx)
        {
            try
            {
                var userAgent = ctx.GetRowUserAgent();
                var split = userAgent.Split(';');
                var applicationVersion = split.Length >= 8 ? split[0] : string.Empty;
                return applicationVersion;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        
        public static string GetInstallationId(this HttpContext ctx)
        {
            try
            {
                var userAgent = ctx.GetRowUserAgent();
                var split = userAgent.Split(';');
                var installationId = split.Length >= 9 ? split[8] : string.Empty;
                return installationId;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public static bool TryGetUserAgentData(this HttpContext ctx, out UserAgentData userAgentData)
        {
            try
            {
                var userAgent = ctx.GetRowUserAgent();
                var split = userAgent.Split(';');

                if (split.Length < 9)
                {
                    userAgentData = null;
                    return false;
                }

                userAgentData = new UserAgentData()
                {
                    ApplicationVersion = split[0],
                    Build = split[1],
                    Platform = split[2],
                    ScreenSize = split[3],
                    PixelRatio = split[4],
                    Lang = split[5].Length == 2 ? split[5] : string.Empty,
                    DeviceModel = split[6],
                    DeviceUid = split[7],
                    InstallationId = split[8]
                };
                
                return true;
            }
            catch (Exception e)
            {
                userAgentData = null;
                return false;
            }
        }
    }
}