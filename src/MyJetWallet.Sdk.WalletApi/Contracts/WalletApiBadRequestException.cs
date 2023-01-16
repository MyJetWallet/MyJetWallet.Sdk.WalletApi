using System.Net;

namespace MyJetWallet.Sdk.WalletApi.Contracts
{
    public class WalletApiBadRequestException: WalletApiHttpException
    {
        public WalletApiBadRequestException(string message) : base(message, HttpStatusCode.BadRequest)
        {
        }
    }
}