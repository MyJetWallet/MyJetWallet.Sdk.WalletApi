using System.Collections.Generic;
using System.Threading.Tasks;
using MyJetWallet.Domain;
using Service.AssetsDictionary.Domain.Models.PaymentMethodsV2;

namespace MyJetWallet.Sdk.WalletApi.Services;

public interface IPaymentService
{
    ValueTask<List<ReceiveMethodModelDTO>> GetReceiveMethodsAsync(JetClientIdentity clientId);
    ValueTask<List<BuyMethodModelDTO>> GetBuyMethodsAsync(JetClientIdentity clientId);
    ValueTask<List<SendMethodModelDTO>> GetSendMethodsAsync(JetClientIdentity clientId);
}