using System;

namespace MyJetWallet.Sdk.WalletApi.Contracts
{
    public class WalletApiErrorException : Exception
    {
        public ApiResponseCodes Code { get; set; }
        public string[] TemplateParams { get; set; }

        public WalletApiErrorException(string message, ApiResponseCodes code, params string[] templateParams) : base(message)
        {
            Code = code;
            TemplateParams = templateParams;
        }
        public WalletApiErrorException(ApiResponseCodes code, params string[] templateParams)
        {
            Code = code;
            TemplateParams = templateParams;
        }
    }
}