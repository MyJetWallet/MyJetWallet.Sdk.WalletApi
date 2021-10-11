﻿using System;
using System.Linq;
using System.ServiceModel.Dispatcher;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyJetWallet.Domain;
using MyJetWallet.Sdk.Authorization.Http;
using MyJetWallet.Sdk.WalletApi.Wallets;
using Newtonsoft.Json;

namespace MyJetWallet.Sdk.WalletApi
{
    public static class ControllerUtils
    {
        public static IWalletService WalletService { get; set; }

        /// <summary>
        /// PrintToken
        /// </summary>
        /// <param name="tokenString"></param>
        /// <returns></returns>
        public static string PrintToken(this string tokenString)
        {
            try
            {
                var (result, token) = MyControllerBaseHelper.ParseToken(tokenString);

                return JsonConvert.SerializeObject(token);
            }
            catch (Exception)
            {
                return "Invalid token";
            }
        }

        /// <summary>
        /// User agent of request
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static string GetUserAgent11(this HttpContext ctx)
        {
            try
            {
                return ctx.Request.Headers["User-Agent"];
            }
            catch (Exception)
            {
                return "Unknown";
            }
        }

        public static JetClientIdentity GetClientIdentity(this ControllerBase controller)
        {
            var id = new JetClientIdentity(controller.GetBrokerId(), controller.GetBrandId(), controller.GetClientId());
            return id;
        }

        private static readonly string[] IpHeaders = new string[4]
        {
            "CF-Connecting-IP",
            "X-Forwarded-For",
            "HTTP_X_FORWARDED_FOR",
            "REMOTE_ADDR"
        };
        
        public static string ClientIp(this ControllerBase controller)
        {
            var httpRequest = controller.HttpContext.Request;
            
            foreach (string ipHeader in IpHeaders)
            {
                if (httpRequest.Headers.ContainsKey(ipHeader))
                {
                    var value = httpRequest.Headers[ipHeader].ToString();
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        var firstItem = value.Split(";").First().Split(",").First();
                        return firstItem;
                    }
                }
            }
            
            return httpRequest.HttpContext.Connection.RemoteIpAddress.ToString();
        }

    }
}