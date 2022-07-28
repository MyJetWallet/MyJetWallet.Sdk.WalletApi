using Microsoft.AspNetCore.Authorization;
using MyJetWallet.Sdk.Authorization;

namespace MyJetWallet.Sdk.WalletApi.Common
{
    public static class AuthorizationPolicies
    {
        public const string VerifiedEmailPolicy = "VerifiedEmail";
        public const string Passed2FaPolicy = "Passed2Fa";
        public const string PassedKYCPolicy = "PassedKYC";
        public const string SessionCheckPassPolicy = "SessionCheckPass";
        public static void SetupWalletApiPolicy(this AuthorizationOptions options)
        {
            options.AddPolicy(VerifiedEmailPolicy, policy => policy.RequireClaim("Email-Verified", "True"));
            options.AddPolicy(Passed2FaPolicy, policy =>
            {
                policy.RequireClaim("Email-Verified", "True");
                policy.RequireClaim("2FA-Passed", "True");
            });
            options.AddPolicy(PassedKYCPolicy,policy => policy.RequireClaim("KYCPassed", "True")); 
            
            options.AddPolicy(SessionCheckPassPolicy, policy =>
            {
                policy.RequireClaim(AuthorizationConst.SetupPinPassClaim, "True");
                policy.RequireClaim(AuthorizationConst.VerifyPinPassClaim, "True");
                policy.RequireClaim(AuthorizationConst.ProfileKycPassClaim, "True");
                policy.RequireClaim(AuthorizationConst.SelfiePassClaim, "True");
            });
        }
    }
}