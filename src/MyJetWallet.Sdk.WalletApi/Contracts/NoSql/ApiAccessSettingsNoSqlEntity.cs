using System;
using MyNoSqlServer.Abstractions;
using Service.AssetsDictionary.Domain.Models;
using Service.AssetsDictionary.MyNoSql;

namespace MyJetWallet.Sdk.WalletApi.Contracts.NoSql;

public class ApiAccessSettingsNoSqlEntity: MyNoSqlDbEntity
{
    public const string TableName = "myjetwallet-api-access-settings";

    public static string GeneratePartitionKey() => "ApiAccessSettings";
    public static string GenerateRowKey() => "ApiAccessSettings";

    public bool EnableBlocksWithoutCountries { get; set; }
    public TimeSpan BlockDuration { get; set; }

    public static ApiAccessSettingsNoSqlEntity Create(bool enableBlocksWithoutCountries, TimeSpan blockDuration)
    {
        return new ApiAccessSettingsNoSqlEntity()
        {
            PartitionKey = GeneratePartitionKey(),
            RowKey = GenerateRowKey(),
            EnableBlocksWithoutCountries = enableBlocksWithoutCountries,
            BlockDuration = blockDuration,
        };
    }

}