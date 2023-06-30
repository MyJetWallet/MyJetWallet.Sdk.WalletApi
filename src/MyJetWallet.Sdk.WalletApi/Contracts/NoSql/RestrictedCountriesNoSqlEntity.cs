using MyNoSqlServer.Abstractions;
using Service.AssetsDictionary.Domain.Models;
using Service.AssetsDictionary.MyNoSql;

namespace MyJetWallet.Sdk.WalletApi.Contracts.NoSql;

public class RestrictedCountriesNoSqlEntity: MyNoSqlDbEntity
{
    public const string TableName = "myjetwallet-restricted-countries";

    public static string GeneratePartitionKey() => "RedistrictedCountries";
    public static string GenerateRowKey(string countryCode) => countryCode;

    public string CountryCode { get; set; }

    public static RestrictedCountriesNoSqlEntity Create(string countryCode)
    {
        return new RestrictedCountriesNoSqlEntity()
        {
            PartitionKey = GeneratePartitionKey(),
            RowKey = GenerateRowKey(countryCode),
            CountryCode = countryCode,
        };
    }

}