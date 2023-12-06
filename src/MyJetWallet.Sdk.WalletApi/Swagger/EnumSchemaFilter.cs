using System;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyJetWallet.Sdk.WalletApi.Swagger;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum)
            return;

        model.Enum.Clear();
        foreach (var enumValue in Enum.GetValues(context.Type))
        {
            var enumName = Enum.GetName(context.Type, enumValue);

            var memberInfo = enumName == null
                ? null
                : context.Type.GetMember(enumName).FirstOrDefault(m => m.DeclaringType == context.Type);
            
            var enumMemberAttribute = memberInfo == null
                ? null
                : memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false).OfType<EnumMemberAttribute>()
                    .FirstOrDefault();

            var label = enumMemberAttribute == null || string.IsNullOrWhiteSpace(enumMemberAttribute.Value)
                ? enumName
                : enumMemberAttribute.Value;

            label = $"{label} ({(int) enumValue})";
            model.Enum.Add(new OpenApiString(label));
        }
    }
}