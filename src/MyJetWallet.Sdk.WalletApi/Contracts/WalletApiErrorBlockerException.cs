﻿using System;

namespace MyJetWallet.Sdk.WalletApi.Contracts
{
    public class WalletApiErrorBlockerException : Exception
    {
        public ApiResponseCodes Code { get; set; }
        public UnauthorizedData UnauthorizedData { get; set; }
        public string[] TemplateParams { get; set; }

        public WalletApiErrorBlockerException(ApiResponseCodes code, string message, int attempts, params string[] templateParams) : base(message)
        {
            Code = code;
            UnauthorizedData = new UnauthorizedData {Attempts = new AttemptsData()
            {
                Left = attempts
            }};
            TemplateParams = templateParams;

        }
        public WalletApiErrorBlockerException(ApiResponseCodes code, string message, TimeSpan timeSpan, params string[] templateParams) : base(message)
        {
            Code = code;
            UnauthorizedData = new UnauthorizedData {Blocker = new BlockerExpiredData()
            {
                Expired = timeSpan
            }};   
            TemplateParams = templateParams;

        }
        public WalletApiErrorBlockerException(ApiResponseCodes code, string message, params string[] templateParams) : base(message)
        {
            Code = code;
            TemplateParams = templateParams;
        }
    }
}