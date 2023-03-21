using System.Collections.Generic;
using System.Threading.Tasks;
using MyJetWallet.Domain;
using Service.AssetsDictionary.Client;
using Service.AssetsDictionary.Domain.Models.PaymentMethodsV2;
using Service.KYC.Client;
using Service.KYC.Grpc.Models;

namespace MyJetWallet.Sdk.WalletApi.Services;


public class PaymentService : IPaymentService
{
    private readonly IKycStatusClient _kycStatusClient;
    private readonly IPaymentMethodV2Client _paymentMethodV2Client;

    public PaymentService(IKycStatusClient kycStatusClient, 
        IPaymentMethodV2Client paymentMethodV2Client)
    {
        _kycStatusClient = kycStatusClient;
        _paymentMethodV2Client = paymentMethodV2Client;
    }

    public async ValueTask<List<ReceiveMethodModelDTO>> GetReceiveMethodsAsync(JetClientIdentity clientId)
    {
        var kycStatus = await _kycStatusClient.GetClientKycStatus(new KycStatusRequest()
        {
            BrokerId = clientId.BrokerId,
            ClientId = clientId.ClientId
        });
        var country = kycStatus.Country;
            
        var paymentSettings = await _paymentMethodV2Client.GetPaymentMethods(clientId.ClientId, country);
        return paymentSettings.ReceiveMethods;
    }
    
    public async ValueTask<List<BuyMethodModelDTO>> GetBuyMethodsAsync(JetClientIdentity clientId)
    {
        var kycStatus = await _kycStatusClient.GetClientKycStatus(new KycStatusRequest()
        {
            BrokerId = clientId.BrokerId,
            ClientId = clientId.ClientId
        });
        var country = kycStatus.Country;
            
        var paymentSettings = await _paymentMethodV2Client.GetPaymentMethods(clientId.ClientId, country);
        return paymentSettings.BuyMethods;
    }
    
    public async ValueTask<List<SendMethodModelDTO>> GetSendMethodsAsync(JetClientIdentity clientId)
    {
        var kycStatus = await _kycStatusClient.GetClientKycStatus(new KycStatusRequest()
        {
            BrokerId = clientId.BrokerId,
            ClientId = clientId.ClientId
        });
        var country = kycStatus.Country;
            
        var paymentSettings = await _paymentMethodV2Client.GetPaymentMethods(clientId.ClientId, country);
        return paymentSettings.SendMethods;
    }
}