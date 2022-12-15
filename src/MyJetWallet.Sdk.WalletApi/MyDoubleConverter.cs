using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyJetWallet.Sdk.WalletApi
{
    public class MyDoubleConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var d = reader.GetDecimal();

            return (double)d;
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue((decimal) value);
        }
    }
}