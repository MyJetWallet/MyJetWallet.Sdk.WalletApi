using MyNoSqlServer.Abstractions;
using Service.AssetsDictionary.Domain.Models;
using Service.AssetsDictionary.MyNoSql;

namespace MyJetWallet.Sdk.WalletApi.Contracts.NoSql;

public class RestrictedMethodsNoSqlEntity: MyNoSqlDbEntity
{
    public const string TableName = "myjetwallet-restricted-methods";
    public static string GeneratePartitionKey() => "RedistrictedMethods";
    public static string GenerateRowKey(string path) => path;
    
    public string Path { get; set; }
    public int Attempts5Sec { get; set; }
    public int Attempts1Min { get; set; }
    public int Attempts1Hour { get; set; }
    public int Attempts1Day { get; set; }
    
    public static RestrictedMethodsNoSqlEntity Create(string path, int attempts5Sec, int attempts1Min, int attempts1Hour, int attempts1day)
    {
        return new RestrictedMethodsNoSqlEntity()
        {
            PartitionKey = GeneratePartitionKey(),
            RowKey = GenerateRowKey(path),
            Path = path,
            Attempts5Sec = attempts5Sec,
            Attempts1Min = attempts1Min,
            Attempts1Hour = attempts1Hour,
            Attempts1Day = attempts1day,
        };
    }
}