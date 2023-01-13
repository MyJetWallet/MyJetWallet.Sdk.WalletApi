using System.Net;

namespace MyJetWallet.Sdk.WalletApi.Contracts
{
    public class WalletApiBadRequestException: WalletApiHttpException
    {
        public WalletApiBadRequestException(string localizedMessage) : base(localizedMessage, HttpStatusCode.BadRequest)
        {
        }
    }
}