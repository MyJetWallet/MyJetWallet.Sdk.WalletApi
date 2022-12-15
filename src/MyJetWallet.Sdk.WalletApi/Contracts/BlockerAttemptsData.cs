namespace MyJetWallet.Sdk.WalletApi.Contracts;

public class BlockerAttemptsData
{
    public int CurrentAttempts { get; set; }
    public int MaxAttempts { get; set; }
}