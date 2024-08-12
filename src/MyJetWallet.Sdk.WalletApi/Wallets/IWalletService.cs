using System.Collections.Generic;
using System.Threading.Tasks;
using MyJetWallet.Domain;
using Service.ClientWallets.Domain.Models;

namespace MyJetWallet.Sdk.WalletApi.Wallets
{
    public interface IWalletService
    {
        ValueTask<List<ClientWallet>> GetWalletsAsync(JetClientIdentity clientId);

        ValueTask<ClientWallet> GetDefaultWalletAsync(JetClientIdentity clientId);
        
        ValueTask<ClientWallet> GetInvestWalletAsync(JetClientIdentity clientId);

        ValueTask<ClientWallet> GetEarnWalletAsync(JetClientIdentity clientId);
        ValueTask<ClientWallet> GetJarWalletAsync(JetClientIdentity clientId);

        ValueTask<ClientWallet> GetWalletByIdAsync(JetClientIdentity clientId, string walletId);
        ValueTask<bool> SetBaseAssetAsync(JetClientIdentity clientId, string walletId, string baseAsset);

        ValueTask<JetWalletIdentity> GetWalletIdentityByIdAsync(JetClientIdentity clientId, string walletId);
    }
}