using Microsoft.AspNetCore.Authorization;

namespace MyJetWallet.Sdk.WalletApi.Common;

public class JetSessionAuthorizeAttribute : AuthorizeAttribute
{
    public JetSessionAuthorizeAttribute()
    {
        Policy = AuthorizationPolicies.SessionCheckPassPolicy;
    }
}