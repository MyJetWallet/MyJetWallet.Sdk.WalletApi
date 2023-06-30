using MyNoSqlServer.Abstractions;
using Service.AssetsDictionary.Domain.Models;
using Service.AssetsDictionary.MyNoSql;

namespace MyJetWallet.Sdk.WalletApi.Contracts.NoSql;

public class RestrictedIPsNoSqlEntity: MyNoSqlDbEntity
{
    public const string TableName = "myjetwallet-restricted-ips";
    public static string GeneratePartitionKey() => "RedistrictedIps";
    public static string GenerateRowKey(string ip) => ip;
    
    public string IpAddress { get; set; }
    
    public int Attempts5Sec { get; set; }
    public int Attempts1Min { get; set; }
    public int Attempts1Hour { get; set; }
    public int Attempts1Day { get; set; }
    
    public static RestrictedIPsNoSqlEntity Create(string ip,  int attempts5Sec, int attempts1Min, int attempts1Hour, int attempts1day)
    {
        return new RestrictedIPsNoSqlEntity()
        {
            PartitionKey = GeneratePartitionKey(),
            RowKey = GenerateRowKey(ip),
            IpAddress = ip,
            Attempts5Sec = attempts5Sec,
            Attempts1Min = attempts1Min,
            Attempts1Hour = attempts1Hour,
            Attempts1Day = attempts1day,
        };
    }
}