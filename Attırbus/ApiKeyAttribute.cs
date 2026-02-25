using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    private const string APIKEYNAME = "X-Api-Key"; 

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        
        if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
        {
            context.Result = new ContentResult() { StatusCode = 401, Content = "API Key eksik!" };
            return;
        }

        
        var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var apiKey = appSettings.GetValue<string>("ApiKeySettings:ApiKey");

        if (!apiKey.Equals(extractedApiKey))
        {
            context.Result = new ContentResult() { StatusCode = 403, Content = "Yetkisiz Erişim! Anahtar Yanlış." };
            return;
        }

        await next();
    }
}