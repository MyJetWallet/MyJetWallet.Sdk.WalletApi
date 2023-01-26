using System;
using System.Linq;
using System.Text.Json;
using MyJetWallet.Sdk.WalletApi.Contracts;
using Newtonsoft.Json;
using NUnit.Framework;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MyJetWallet.Sdk.WalletApi.Tests
{
    
    public class Class1
    {
        [Test]
        public void Test1()
        {
            var data = new Data()
            {
                Name = "data",
                V1 = 1m/3m,
                V2 = 1.0/3.0,
                V3 = 1111111
            };

            var json = JsonSerializer.Serialize(data, typeof(Data), new JsonSerializerOptions()
            {
                Converters =
                {
                    new MyDoubleConverter()
                }
            });

            var d = JsonSerializer.Deserialize<Data>(json, new JsonSerializerOptions()
            {
                Converters =
                {
                    new MyDoubleConverter()
                }
            });
            
            Console.WriteLine(json);

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine(JsonConvert.SerializeObject(d, Formatting.Indented));

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine((data.V2 + data.V2 + data.V2) == 1);
            Console.WriteLine((d.V2 + d.V2 + d.V2) == 1);
        }


        [Test]
        public void Test2()
        {
            Console.WriteLine(true.ToString());
        }

        [Test]
        public void Test3()
        {
            var codes = Enum.GetValues<ApiResponseCodes>().ToList();
            foreach (var code in codes)
            {
                Console.WriteLine($"AddBody(ApiResponseCodes.{code});");
            }
        }
        
        [Test]
        public void Test4()
        {

            var paramValues = new string[] {"123"};
        
            var keys = ApiResponseClassData.TemplateBodyParams[ApiResponseCodes.AmountIsSmall];
            var body = "Placeholder for amountissmall: ${MINAMOUNT}";
            
            for (var i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                body = body.Replace(key, paramValues[i]);
            }

            Console.WriteLine(body);
        }
    }

    public class Data
    {
        public string Name { get; set; }
        public decimal V1 { get; set; }
        public double V2 { get; set; }
        public int V3 { get; set; }
    }
}
