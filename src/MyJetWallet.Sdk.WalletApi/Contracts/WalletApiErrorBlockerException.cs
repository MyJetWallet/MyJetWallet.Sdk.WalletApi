using System;

namespace MyJetWallet.Sdk.WalletApi.Contracts
{
    public class WalletApiErrorBlockerException : Exception
    {
        public ApiResponseCodes Code { get; set; }
        public BlockerExpiredData BlockerData { get; set; }

        public WalletApiErrorBlockerException(string message, ApiResponseCodes code, TimeSpan timeSpan) : base(message)
        {
            Code = code;
            BlockerData = new BlockerExpiredData {BlockerExpired = timeSpan};
        }
        public WalletApiErrorBlockerException(ApiResponseCodes code)
        {
            Code = code;
        }
    }
}