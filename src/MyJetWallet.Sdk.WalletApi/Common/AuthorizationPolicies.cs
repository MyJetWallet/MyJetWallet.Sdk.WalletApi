using Microsoft.AspNetCore.Authorization;

namespace MyJetWallet.Sdk.WalletApi.Common
{
    public static class AuthorizationPolicies
    {
        public const string VerifiedEmailPolicy = "VerifiedEmail";
        public const string Passed2FaPolicy = "Passed2Fa";
        public const string PassedKYCPolicy = "PassedKYC";
        public static void SetupWalletApiPolicy(this AuthorizationOptions options)
        {
            options.AddPolicy(VerifiedEmailPolicy, policy => policy.RequireClaim("Email-Verified", "True"));
            options.AddPolicy(Passed2FaPolicy, policy =>
            {
                policy.RequireClaim("Email-Verified", "True");
                policy.RequireClaim("2FA-Passed", "True");
            });
            options.AddPolicy(PassedKYCPolicy,policy => policy.RequireClaim("KYCPassed", "True")); 
        }
    }
}