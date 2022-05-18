using System;

namespace MyJetWallet.Sdk.WalletApi.Contracts
{
    public class WalletApiErrorBlockerException : Exception
    {
        public ApiResponseCodes Code { get; set; }
        public UnauthorizedData UnauthorizedData { get; set; }

        public WalletApiErrorBlockerException(string message, int attempts) : base(message)
        {
            Code = ApiResponseCodes.InvalidUserNameOrPassword;
            UnauthorizedData = new UnauthorizedData {Attempts = new AttemptsData()
            {
                Left = attempts
            }};
        }
        public WalletApiErrorBlockerException(string message, TimeSpan timeSpan) : base(message)
        {
            Code = ApiResponseCodes.OperationBlocked;
            UnauthorizedData = new UnauthorizedData {Blocker = new BlockerExpiredData()
            {
                Expired = timeSpan
            }};
        }
        public WalletApiErrorBlockerException(ApiResponseCodes code)
        {
            Code = code;
        }
    }
}