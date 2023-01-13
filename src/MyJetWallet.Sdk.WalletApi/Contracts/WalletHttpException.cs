using System;
using System.Net;

namespace MyJetWallet.Sdk.WalletApi.Contracts
{
    public class WalletApiHttpException: Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public WalletApiHttpException(string localizedMessage, HttpStatusCode statusCode) : base(localizedMessage)
        {
            StatusCode = statusCode;
        }
    }
}