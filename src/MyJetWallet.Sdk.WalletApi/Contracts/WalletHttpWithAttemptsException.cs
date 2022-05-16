using System;
using System.Net;

namespace MyJetWallet.Sdk.WalletApi.Contracts
{
    public class WalletHttpWithAttemptsException: Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public BlockerAttemptsData BlockerData { get; set; }

        public WalletHttpWithAttemptsException(string message, HttpStatusCode statusCode, int current, int max) : base(message)
        {
            StatusCode = statusCode;
            BlockerData = new BlockerAttemptsData() {CurrentAttempts = current, MaxAttempts = max};
        }
    }
}