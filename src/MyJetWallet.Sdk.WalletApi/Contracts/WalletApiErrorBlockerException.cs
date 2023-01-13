using System;

namespace MyJetWallet.Sdk.WalletApi.Contracts
{
    public class WalletApiErrorBlockerException : Exception
    {
        public ApiResponseCodes Code { get; set; }
        public UnauthorizedData UnauthorizedData { get; set; }

        public WalletApiErrorBlockerException(string localizedMessage, int attempts) : base(localizedMessage)
        {
            Code = ApiResponseCodes.InvalidUserNameOrPassword;
            UnauthorizedData = new UnauthorizedData {Attempts = new AttemptsData()
            {
                Left = attempts
            }};
        }
        public WalletApiErrorBlockerException(string localizedMessage, TimeSpan timeSpan) : base(localizedMessage)
        {
            Code = ApiResponseCodes.OperationBlocked;
            UnauthorizedData = new UnauthorizedData {Blocker = new BlockerExpiredData()
            {
                Expired = timeSpan
            }};
        }
        public WalletApiErrorBlockerException(ApiResponseCodes code, string localizedMessage) : base(localizedMessage)
        {
            Code = code;
        }
    }
}