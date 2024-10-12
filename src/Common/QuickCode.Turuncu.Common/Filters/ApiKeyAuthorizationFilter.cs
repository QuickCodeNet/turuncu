using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QuickCode.Turuncu.Common.Filters;

public class ApiKeyAuthorizationFilter : IAuthorizationFilter
{
    private const string ApiKeyHeaderName = "X-Api-Key";
    private const string ApiKeyConfigKey = "AppSettings:ApiKey";
    private readonly string _apiSecretKey;
    private readonly IConfiguration _configuration;

    public ApiKeyAuthorizationFilter(IConfiguration configuration)
    {
        _configuration = configuration; 
        _apiSecretKey = $"{_configuration!.GetValue<string>(ApiKeyConfigKey)}";
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var apiKey = context.HttpContext.Request.Headers[ApiKeyHeaderName];

        if (string.IsNullOrEmpty(apiKey))
        {
            context.Result = new BadRequestObjectResult("Api Key Missing! - Bad Request!");
        }

        if (!_apiSecretKey.Equals(apiKey))
        {
            context.Result = new UnauthorizedObjectResult("Invalid Api Key");
        }
    }
}

public class ApiKeyAttribute : ServiceFilterAttribute
{
    public ApiKeyAttribute()
        : base(typeof(ApiKeyAuthorizationFilter))
    {
    }
}