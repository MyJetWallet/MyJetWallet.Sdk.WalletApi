using System;

namespace MyJetWallet.Sdk.WalletApi.Contracts
{
    public class WalletApiErrorException : Exception
    {
        public ApiResponseCodes Code { get; set; }

        public WalletApiErrorException(ApiResponseCodes code, string localizedMessage) : base(localizedMessage)
        {
            Code = code;
        }
        public WalletApiErrorException(ApiResponseCodes code)
        {
            Code = code;
        }
    }
}