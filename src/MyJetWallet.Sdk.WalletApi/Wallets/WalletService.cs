using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.Domain;
using Service.ClientWallets.Domain.Models;
using Service.ClientWallets.Grpc;
using Service.ClientWallets.Grpc.Models;

namespace MyJetWallet.Sdk.WalletApi.Wallets
{
    public class WalletService : IWalletService
    {
        private readonly IClientWalletService _clientWalletService;
        private readonly ILogger<WalletService> _logger;

        public WalletService(IClientWalletService clientWalletService, ILogger<WalletService> logger)
        {
            _clientWalletService = clientWalletService;
            _logger = logger;
        }


        public async ValueTask<List<ClientWallet>> GetWalletsAsync(JetClientIdentity clientId)
        {
            var list = await _clientWalletService.GetWalletsByClient(clientId);
            return list.Wallets;
        }

        public async ValueTask<ClientWallet> GetDefaultWalletAsync(JetClientIdentity clientId)
        {
            var list = await _clientWalletService.GetWalletsByClient(clientId);

            var defaultWallet = list.Wallets.FirstOrDefault(e => e.IsDefault);

            if (defaultWallet == null)
            {
                _logger.LogError("Cannot found default wallet for Broker/Brand/Client: {brokerId}/{brandId}/{clientId}", clientId.BrokerId, clientId.BrandId, clientId.ClientId);
                throw new Exception($"Cannot found default wallet for Broker/Brand/Client: {clientId.BrokerId}/{clientId.BrandId}/{clientId.ClientId}");
            }

            Activity.Current?.AddTag("walletId", defaultWallet.WalletId);
            Activity.Current?.AddBaggage("walletId", defaultWallet.WalletId);

            return defaultWallet;
        }

        public async ValueTask<ClientWallet> GetInvestWalletAsync(JetClientIdentity clientId)
        {
            var list = await _clientWalletService.GetWalletsByClient(clientId);

            //todo: move it to wallet service
            var investWalletName = "Invest";
            var investWalletBaseAsset = "USDT";
            
            var investWallet = list.Wallets.FirstOrDefault(e => e.Name == investWalletName);

            if (investWallet != null)
                return investWallet;
            
            var resp = await _clientWalletService.CreateWalletAsync(new CreateWalletRequest()
            {
                ClientId = clientId,
                Name = investWalletName,
                BaseAsset = investWalletBaseAsset
            });
            
            if (!resp.Success)
                throw new Exception("Cannot create invest wallet");

            var info = await _clientWalletService.GetWalletInfoByIdAsync(new GetWalletInfoByIdRequest()
            {
                WalletId = resp.WalletId
            });

            if (!info.Success)
                throw new Exception("Cannot find after create invest wallet");
            
            investWallet = info.WalletInfo;
            
            return investWallet;
        }

        public async ValueTask<ClientWallet> GetWalletByIdAsync(JetClientIdentity clientId, string walletId)
        {
            var list = await _clientWalletService.GetWalletsByClient(clientId);

            var wallet = list.Wallets.FirstOrDefault(e => e.WalletId == walletId);

            return wallet;
        }

        public async ValueTask<bool> SetBaseAssetAsync(JetClientIdentity clientId, string walletId, string baseAsset)
        {
            var response = await _clientWalletService.SetBaseAssetAsync(new SetBaseAssetRequest()
            {
                ClientId = clientId,
                WalletId = walletId,
                BaseAsset = baseAsset
            });

            return response.Success;
        }

        public async ValueTask<JetWalletIdentity> GetWalletIdentityByIdAsync(JetClientIdentity clientId, string walletId)
        {
            var wallet = await GetWalletByIdAsync(clientId, walletId);

            if (wallet == null)
                return null;

            return new JetWalletIdentity(clientId.BrokerId, clientId.BrandId, clientId.ClientId, wallet.WalletId);
        }
    }
}