using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.WalletApi.Contracts;
using Service.MessageTemplates.Client;
using Service.MessageTemplates.Domain.Models;
using Service.MessageTemplates.Grpc;
using Service.MessageTemplates.Grpc.Models;

namespace MyJetWallet.Sdk.WalletApi;

public class LocalizationManager
{
    private readonly ITemplateService _templateService;
    private readonly ITemplateClient _templateClient;
    private readonly ILogger<LocalizationManager> _logger;
    public LocalizationManager(ITemplateService templateService, ILogger<LocalizationManager> logger, ITemplateClient templateClient)
    {
        _templateService = templateService;
        _logger = logger;
        _templateClient = templateClient;
    }

    public async Task Start()
    {
        var codes = Enum.GetValues<ApiResponseCodes>().ToList();
        foreach (var code in codes.Where(code => !ApiResponseClassData.TemplateBodyParams.TryGetValue(code, out _)))
        {
            _logger.LogError($"Api code {code} doesnt have params");
            throw new Exception($"Api code {code} doesnt have params");
        }
        var templates = await _templateService.GetAllTemplates();
        foreach (var code in codes)
        {
            var template = templates.Templates.FirstOrDefault(t => t.TemplateId == code.ToString().ToLower());
            if (template != null)
                continue;
            
            await _templateService.CreateNewTemplate(new Template
            {
                TemplateId = code.ToString(),
                Params = ApiResponseClassData.TemplateBodyParams[code],
                Type = TemplateType.ApiResponseCode
            });
        }
    }

    public async Task<string> GetTemplateBody(ApiResponseCodes code, HttpContext ctx, params string[] paramValues)
    {
        var lang = ctx.GetLang();
        if (string.IsNullOrWhiteSpace(lang))
            lang = Defaults.DefaultLang;
        
        var body = await _templateClient.GetTemplateBody(code.ToString(), Defaults.DefaultBrand, lang);
        var keys = ApiResponseClassData.TemplateBodyParams[code];
        
        if (!keys.Any()) 
            return body;
        
        for (var i = 0; i < keys.Count; i++)
        {
            var key = keys[i];
            try
            {
                body = body.Replace(key, paramValues[i]);
            }
            catch (Exception e)
            {
                _logger.LogError($"Unable to apply params to temlpate {code.ToString()}, param {key}");
            }
        }

        return body;
    }
}