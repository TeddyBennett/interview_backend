using Backend_Test.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Backend_Test.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private const string ApiKeyHeaderName = "X-API-Key";

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
            {
                context.Result = new UnauthorizedObjectResult("API Key is missing.");
                return;
            }

            var apiKeyService = context.HttpContext.RequestServices.GetRequiredService<IApiKeyService>();
            var apiKey = potentialApiKey.ToString();

            var isValid = await apiKeyService.ValidateApiKeyAsync(apiKey);

            if (!isValid)
            {
                context.Result = new UnauthorizedObjectResult("API Key is not valid.");
                return;
            }

            await Task.CompletedTask;
        }
    }
}
