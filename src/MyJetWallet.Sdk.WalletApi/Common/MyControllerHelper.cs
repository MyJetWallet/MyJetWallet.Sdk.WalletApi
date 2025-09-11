using MyJetWallet.Sdk.WalletApi.Contracts;

namespace MyJetWallet.Sdk.WalletApi.Common;

public static class MyControllerHelper
{
    public static void ValidateShouldNotBeEmpty(this string s, string errorMessage)
    {
        if (string.IsNullOrEmpty(s))
        {
            throw new WalletApiErrorException(errorMessage, ApiResponseCodes.ValidationError);
        }
    }

    public static void ShouldBe(bool result, string paramText, object paramValue)
    {
        if (!result)
        {
            throw new WalletApiErrorException($"Wrong parameter value. {paramText} = {paramValue}", ApiResponseCodes.ValidationError);
        }
    }

    public static bool IsNotEmpty(this string s)
    {
        return !string.IsNullOrEmpty(s);
    }
}