using System;

namespace MyJetWallet.Sdk.WalletApi.Contracts
{
    public class WalletApiErrorAttemptsException : Exception
    {
        public ApiResponseCodes Code { get; set; }
        public BlockerAttemptsData BlockerData { get; set; }

        public WalletApiErrorAttemptsException(string message, ApiResponseCodes code, int current, int max) : base(message)
        {
            Code = code;
            BlockerData = new BlockerAttemptsData() {CurrentAttempts = current, MaxAttempts = max};
        }
        public WalletApiErrorAttemptsException(ApiResponseCodes code)
        {
            Code = code;
        }
    }
}