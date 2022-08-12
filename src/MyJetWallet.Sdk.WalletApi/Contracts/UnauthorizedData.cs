using System;

namespace MyJetWallet.Sdk.WalletApi.Contracts;

public class UnauthorizedData
{
    public BlockerExpiredData Blocker { get; set; }
    public AttemptsData Attempts { get; set; }
    public string ErrorMessage { get; set; }
}
public class BlockerExpiredData
{
    public TimeSpan Expired { get; set; }
}

public class AttemptsData
{
    public int Left { get; set; }
}

