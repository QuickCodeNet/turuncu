using System.Net;
using System.Reflection;
using System.Text.Json.Serialization;
using QuickCode.Turuncu.Common;
using QuickCode.Turuncu.Common.Extensions;
using QuickCode.Turuncu.Gateway.Messaging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using QuickCode.Turuncu.Common.Nswag.Clients.UserManagerModuleApi.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using QuickCode.Turuncu.Common.Controllers;
using Yarp.ReverseProxy.Transforms;
using QuickCode.Turuncu.Common.Helpers;
using QuickCode.Turuncu.Common.Nswag;
using QuickCode.Turuncu.Common.Nswag.Extensions;
using QuickCode.Turuncu.Gateway.Models;
using QuickCode.Turuncu.Gateway.Extensions;
using QuickCode.Turuncu.Gateway.HTTP;
using QuickCode.Turuncu.Gateway.KafkaProducer;
using Serilog;
using InMemoryConfigProvider = QuickCode.Turuncu.Gateway.Extensions.InMemoryConfigProvider;
using JsonSerializer = System.Text.Json.JsonSerializer;

var builder = WebApplication.CreateBuilder(args);

ConfigureEnvironmentVariables(builder.Configuration);

builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

builder.Services.AddSingleton<IKafkaProducerWrapper, KafkaProducerWrapper>();

builder.Services
    .AddReverseProxy()
    .ConfigureHttpClient((context, handler) =>
    {
        handler.AllowAutoRedirect = true;
    })
    .LoadFromMemory()
    .AddTransforms(context =>
    {
        context.RequestTransforms.Add(new RequestHeaderRemoveTransform("Cookie"));
    });

builder.Services.AddControllers().AddJsonOptions(jsonOptions =>
{
    jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddQuickCodeSwaggerGen(builder.Configuration);
builder.Services.AddNswagServiceClient(builder.Configuration, typeof(QuickCodeBaseApiController));
builder.Services.AddCustomHealthChecks(builder.Configuration);
builder.Services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);
var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();


var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
Log.Information($"Started({environmentName} Port:{port})...");

var elasticConnectionString = Environment.GetEnvironmentVariable("ELASTIC_CONNECTION_STRING");

if (!string.IsNullOrEmpty(elasticConnectionString))
{
    builder.Configuration["Logging:ElasticConfiguration:Uri"] = elasticConnectionString;
    Log.Information($"Elastic Connection String updated via Environment Variables.");
}

builder.Services.AddLogger(builder.Configuration);
Log.Information($"{builder.Configuration["Logging:ApiName"]} Started.");

ConfigureMiddlewares();

await app.RunAsync();

void ConfigureMiddlewares()
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseSwagger();
    app.UseSwaggerUI();

    if (app.Configuration.GetValue<bool>("AppSettings:UseHealthCheck"))
    {
        app.UseHealthChecks("/hc", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseHealthChecksUI(config => { config.UIPath = "/hc-ui"; });
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    var gatewayGroup = app.MapGroup("/api/gateway").WithTags("Gateway");
    app.MapGet("/", GetServicesHtml).WithTags("Dashboard");

    gatewayGroup.MapGet("/reset", () =>
    {
        if (InMemoryConfigProvider.IsClustersUpdatedFromConfig == 1)
        {
            InMemoryConfigProvider.IsClustersUpdatedFromConfig = -1;
        }
    });

    gatewayGroup.MapGet("/config", () => InMemoryConfigProvider.proxyConfig.ReverseProxy.ToJson());

    gatewayGroup.MapGet("/swagger-config", () => InMemoryConfigProvider.swaggerMaps.ToJson());

    InMemoryConfigProvider.app = app;
  
    app.MapReverseProxy(proxyPipeline =>
    {
        proxyPipeline.Use(YarpMiddlewareKafkaManager(app.Services));
        proxyPipeline.Use(YarpMiddlewareApiAuthorization(app.Services));
    });

    var corsAllowedUrls = app.Configuration.GetSection("CorsSettings:AllowOrigins").Get<string[]>();
    app.UseCors(x => x
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins(corsAllowedUrls!)
        .SetIsOriginAllowedToAllowWildcardSubdomains());
}

void ConfigureEnvironmentVariables(IConfiguration configuration)
{
    var readConnectionString = Environment.GetEnvironmentVariable("READ_CONNECTION_STRING");
    var writeConnectionString = Environment.GetEnvironmentVariable("WRITE_CONNECTION_STRING");
    var elasticConnectionString = Environment.GetEnvironmentVariable("ELASTIC_CONNECTION_STRING");
    var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

    Log.Information($"Started({environmentName})...");
    Log.Information($"ReadConnectionString = {readConnectionString}");
    Log.Information($"WriteConnectionString = {writeConnectionString}");

    if (!string.IsNullOrEmpty(readConnectionString))
    {
        configuration["ConnectionStrings:ReadConnection"] = readConnectionString;
        Log.Information($"ReadConnection String updated via Environment Variables.");
    }
    
    if (!string.IsNullOrEmpty(writeConnectionString))
    {
        configuration["ConnectionStrings:WriteConnection"] = writeConnectionString;
        Log.Information($"WriteConnection String updated via Environment Variables.");
    }

    if (!string.IsNullOrEmpty(elasticConnectionString))
    {
        configuration["Logging:ElasticConfiguration:Uri"] = elasticConnectionString;
        Log.Information($"Elastic Connection String updated via Environment Variables.");
    }

    builder.Services.AddAuthorizationBuilder()
        .AddPolicy("QuickCodeGatewayPolicy", policy =>
        {
            policy.RequireAssertion(async context =>
            {
                var httpContext = context.Resource as HttpContext;
                if (httpContext == null)
                {
                    return false;
                }

                var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (!string.IsNullOrEmpty(token) && token.IsTokenExpired())
                {
                    httpContext.Response.Headers.Append("Token-Expired", "true");
                }

                return true;
            });
        });

    Console.WriteLine($"environmentName : {environmentName}");
}

async Task<bool> GetTokenIsValid(IServiceProvider services, string token)
{
    var authenticationsClient = services.GetRequiredService<IAuthenticationsClient>();
    var isValidToken = !token.IsTokenExpired() && await authenticationsClient.ValidateAsync(token);
    return isValidToken;
}

Func<HttpContext, Func<Task>, Task> YarpMiddlewareKafkaManager(IServiceProvider services)
{
    return async (context, next) =>
    {
        var memoryCache = services.GetRequiredService<IMemoryCache>();
        var kafkaProducer = services.GetRequiredService<IKafkaProducerWrapper>();
        var originalBodyStream = context.Response.Body;
        context.Request.EnableBuffering();
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await next();
        
        var kafkaEvent = await CheckKafkaEventExists(services, memoryCache, context);
        if (kafkaEvent is not null)
        {
            var requestBodyText = await context.TryGetRequestBodyAsync();
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();
            
            var kafkaMessage = new KafkaMessage
            {
                RequestInfo = new RequestInfo
                {
                    Path = context.Request.Path,
                    Method = context.Request.Method,
                    Headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                    Body = requestBodyText
                },
                ResponseInfo = new ResponseInfo
                {
                    StatusCode = context.Response.StatusCode,
                    Headers = context.Response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                    Body = responseBodyText
                },
                Timestamp = DateTime.UtcNow
            };

            var topic = kafkaEvent.TopicName;
            var key = GenerateKey(context);
            SendKafkaMessage(kafkaProducer, topic, key, kafkaMessage);
        }

        responseBody.Seek(0, SeekOrigin.Begin);
        await responseBody.CopyToAsync(originalBodyStream);
    };
}

Func<HttpContext, Func<Task>, Task> YarpMiddlewareApiAuthorization(IServiceProvider services)
{
    return async (context, next) =>
    {
        var memoryCache = services.GetRequiredService<IMemoryCache>();
        var token = ExtractToken(context);
        var cacheKey = $"AuthJwtTokens-{token}";

        if (HandleTokenExpiration(token, memoryCache, cacheKey))
        {
            await next();
            return;
        }

        if (string.IsNullOrEmpty(token))
        {
            HandleEmptyToken(context, memoryCache, cacheKey);
            await next();
            return;
        }

        if (!await ValidateAndProcessToken(context, services, token, cacheKey))
        {
            return;
        }

        await next();
    };
}


async Task<KafkaEventsGetKafkaEventsResponseDto?> CheckKafkaEventExists(IServiceProvider services,
    IMemoryCache memoryCache, HttpContext context)
{
    var allKafkaEvents = await GetKafkaEvents(services, memoryCache);

    var kafkaEvent = allKafkaEvents.Find(endpoint =>
        endpoint.Path.IsRouteMatch(context.Request.Path) &&
        endpoint.HttpMethod.Equals(context.Request.Method, StringComparison.OrdinalIgnoreCase));

    if (kafkaEvent == null)
    {
        return null;
    }

    var eventName = GetEventName(kafkaEvent, context);
    if (string.IsNullOrEmpty(eventName))
    {
        return null;
    }

    kafkaEvent = KafkaEventsGetKafkaEventsResponseDto.FromJson(kafkaEvent.ToJson());
    var eventKey = $"_{kafkaEvent.HttpMethod}__{eventName}".ToLowerInvariant();
    kafkaEvent!.TopicName = $"{kafkaEvent!.TopicName}{eventKey}".ToLowerInvariant();
    return kafkaEvent;
}

string GetEventName(KafkaEventsGetKafkaEventsResponseDto? kafkaEvent, HttpContext context)
{
    var completeHttpStatusCodes = new List<HttpStatusCode>()
        { HttpStatusCode.OK, HttpStatusCode.NoContent, HttpStatusCode.Accepted, HttpStatusCode.Created };
    var errorHttpStatusCodes = new List<HttpStatusCode>()
    {
        HttpStatusCode.NotFound, HttpStatusCode.BadRequest, HttpStatusCode.Forbidden, HttpStatusCode.Unauthorized,
        HttpStatusCode.InternalServerError, HttpStatusCode.ServiceUnavailable
    };
    var timeoutHttpStatusCodes = new List<HttpStatusCode>()
        { HttpStatusCode.GatewayTimeout, HttpStatusCode.RequestTimeout };

    var eventName =
        kafkaEvent!.OnComplete &&
        completeHttpStatusCodes.Contains((HttpStatusCode)context.Response.StatusCode)
            ? "on_complete"
            : string.Empty;
    
    if (string.IsNullOrEmpty(eventName))
    {
        eventName = kafkaEvent!.OnError &&
                    errorHttpStatusCodes.Contains((HttpStatusCode)context.Response.StatusCode)
            ? "on_error"
            : string.Empty;
    }

    if (string.IsNullOrEmpty(eventName))
    {
        eventName = kafkaEvent!.OnTimeout &&
                    timeoutHttpStatusCodes.Contains((HttpStatusCode)context.Response.StatusCode)
            ? "on_timeout"
            : string.Empty;
    }

    return eventName;
}

string GenerateKey(HttpContext context)
{
    var timestamp = DateTime.UtcNow.Ticks;
    return $"{context.Request.Method}|{context.Request.Path}|{timestamp}";
}

void SendKafkaMessage(IKafkaProducerWrapper kafkaProducer, string topic, string key, KafkaMessage message)
{
    Task.Run(async () =>
    {
        try
        {
            await kafkaProducer.ProduceAsync(topic, key, JsonSerializer.Serialize(message));
            Console.WriteLine($"Message sent to Kafka topic: {topic}, Key: {key}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send message to Kafka. Topic: {topic}, Key: {key}, Error: {ex.Message}");
        }
    });
}

string ExtractToken(HttpContext context)
{
    return context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last() ?? string.Empty;
}

bool HandleTokenExpiration(string token, IMemoryCache memoryCache, string cacheKey)
{
    if (!string.IsNullOrEmpty(token) && token.IsTokenExpired())
    {
        memoryCache.Remove(cacheKey);
        return true;
    }
    
    return false;
}

void HandleEmptyToken(HttpContext context, IMemoryCache memoryCache, string cacheKey)
{
    if (!context.Request.Path.Value!.StartsWith("/api/auth/logout"))
    {
        memoryCache.Remove(cacheKey);
    }
}

async Task<bool> ValidateAndProcessToken(HttpContext context, IServiceProvider services, string token, string cacheKey)
{
    var memoryCache = services.GetRequiredService<IMemoryCache>();
    var configuration = services.GetRequiredService<IConfiguration>();

    if (token.IsTokenExpired())
    {
        context.Response.Headers.Append("Token-Expired", "true");
    }

    var isValidToken = await ValidateToken(services, token, cacheKey, memoryCache);
    if (!isValidToken)
    {
        await HandleInvalidToken(context);
        return false;
    }

    var jwtClaims = token.ParseJwtPayload();
    var permissionGroupId = Convert.ToInt32(jwtClaims["PermissionGroupId"]);

    if (!await IsMethodValid(context, services, permissionGroupId, memoryCache))
    {
        await HandleInvalidMethod(context);
        return false;
    }

    AppendApiKey(context, configuration);
    return true;
}

async Task<bool> ValidateToken(IServiceProvider services, string token, string cacheKey, IMemoryCache memoryCache)
{
    return !token.IsTokenExpired() && await memoryCache.GetOrAddAsync<bool>(cacheKey,
        async parameters => await GetTokenIsValid(services, token),
        TimeSpan.FromSeconds(token.GetTokenExpirationTime()));
}

async Task HandleInvalidToken(HttpContext context)
{
    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    await context.Response.WriteAsync("Token is invalid");
}

async Task<bool> IsMethodValid(HttpContext context, IServiceProvider services, int permissionGroupId, IMemoryCache memoryCache)
{
    var groupMethods = await GetGroupMethods(services, memoryCache);
    var path = context.Request.Path.Value!;
    var validPaths = groupMethods
        .Where(i => i.PermissionGroupId == permissionGroupId && i.HttpMethod == context.Request.Method)
        .Select(i => i.Path)
        .ToList();

    return path.IsRouteMatch(validPaths);
}

async Task<List<KafkaEventsGetKafkaEventsResponseDto>> GetKafkaEvents(IServiceProvider services,
    IMemoryCache memoryCache)
{
    var configuration = services.GetRequiredService<IConfiguration>();
    var kafkaEventsClient = services.GetRequiredService<IKafkaEventsClient>();

    return await memoryCache.GetOrAddAsync<List<KafkaEventsGetKafkaEventsResponseDto>>("KafkaEvents",
        async parameters => await GetAllKafkaEvents(configuration, kafkaEventsClient),
        TimeSpan.FromMinutes(1));
}

async Task<List<GroupHttpMethodPath>> GetGroupMethods(IServiceProvider services, IMemoryCache memoryCache)
{
    var configuration = services.GetRequiredService<IConfiguration>();
    var apiPermissionGroupsClient = services.GetRequiredService<IApiPermissionGroupsClient>();
    var apiMethodDefinitionsClient = services.GetRequiredService<IApiMethodDefinitionsClient>();

    return await memoryCache.GetOrAddAsync<List<GroupHttpMethodPath>>("GroupMethods",
        async parameters => await GetAllGroupMethods(configuration, apiPermissionGroupsClient, apiMethodDefinitionsClient),
        TimeSpan.FromMinutes(1));
}

async Task HandleInvalidMethod(HttpContext context)
{
    context.Response.StatusCode = StatusCodes.Status403Forbidden;
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = "Method Is Not Valid" }));
}

void AppendApiKey(HttpContext context, IConfiguration configuration)
{
    var moduleName = context.GetEndpoint()!.DisplayName!.KebabCaseToPascal("");
    var apiKeyConfigValue = $"QuickCodeApiKeys:{moduleName}ApiKey";
    var configApiKey = configuration.GetValue<string>(apiKeyConfigValue);

    if (configApiKey != null)
    {
        context.Request.Headers.Append("X-Api-Key", configApiKey);
    }
}

IResult GetServicesHtml()
{
    var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
    var portalUrl =  app.Configuration.GetSection("AppSettings:PortalUrl").Get<string>();
    var elasticUrl =  app.Configuration.GetSection("AppSettings:ElasticUrl").Get<string>();
    var kafdropUrl =  app.Configuration.GetSection("AppSettings:KafdropUrl").Get<string>();
    var eventListenerUrl =  app.Configuration.GetSection("AppSettings:EventListenerUrl").Get<string>();
    const string swaggerJson = "/v1/swagger.json";
    const string swaggerHtml = "/index.html";
    var destinations = InMemoryConfigProvider.swaggerMaps.Select(c => new
    {
        ClusterId = c.Key,
        Address = c.Value.Endpoint.Replace(swaggerJson, swaggerHtml)
    }).ToList();
    
    destinations.Add(new { ClusterId = "Event Listener Service", Address = $"{eventListenerUrl}/swagger/index.html" }!);

    var tabsComboBoxHtml = destinations.Select((value, index) => $"<li><a data-toggle=\"tab\"  class=\"dropdown-item\" href=\"{value.Address}\">{value.ClusterId.KebabCaseToPascal()}</a></li>");

    var lastUpdate = DateTime.Now - InMemoryConfigProvider.LastUpdateDateTime;
    var lastUpdateValue = $"{lastUpdate.TotalSeconds:0}s ago";
    if (lastUpdate.Minutes > 0)
    {
        lastUpdateValue = $"{lastUpdate.TotalMinutes:0}m {(lastUpdate.TotalSeconds % 60):0}s ago";
    }

    if (lastUpdate.Hours > 0)
    {
        lastUpdateValue = $"{lastUpdate.TotalHours:0}h {(lastUpdate.TotalMinutes % 60):0}m ago";
    }

    var projectName = typeof(ReverseProxyConfigModel).Namespace!.Split(".")[1];
    var fileContent = File.ReadAllText("Dashboard/Dashboard.html");
    var tabsContent = string.Join("<li><hr class=\"dropdown-divider\"></li>", tabsComboBoxHtml.ToArray());

    var githubUrl = $"https://github.com/QuickCodeNet/{projectName.ToLower()}";
    var isHttpsText = "<meta http-equiv=\"Content-Security-Policy\" content=\"upgrade-insecure-requests\">";
    
    if (environmentName == "Local")
    {
        isHttpsText = "";
    }
    
    isHttpsText = "";
    fileContent = fileContent.Replace("<!--|@TABS@|-->", string.Join("", tabsContent));
    fileContent = fileContent.Replace("<!--|@TABS_COUNT@|-->", destinations.Count().ToString());
    fileContent = fileContent.Replace("<!--|@LAST_UPDATE@|-->", lastUpdateValue);
    fileContent = fileContent.Replace("<!--|@ENVIRONMENT@|-->", environmentName);
    fileContent = fileContent.Replace("<!--|@PROJECT_NAME@|-->", projectName);
    fileContent = fileContent.Replace("<!--|@PORTAL_URL@|-->", portalUrl);
    fileContent = fileContent.Replace("<!--|@ELASTIC_URL@|-->", elasticUrl);
    fileContent = fileContent.Replace("<!--|@GITHUB_URL@|-->", githubUrl);
    fileContent = fileContent.Replace("<!--|@KAFDROP_URL@|-->", kafdropUrl);
    fileContent = fileContent.Replace("<!--|@EVENT_LISTENER_URL@|-->", eventListenerUrl);
    fileContent = fileContent.Replace("<!--|@VERSION@|-->", $"{Assembly.GetExecutingAssembly().GetName().Version}");
    fileContent = fileContent.Replace("<!--|@IS_HTTPS@|-->", isHttpsText);

    return Results.Extensions.Html(@$"{fileContent}");
}

async Task<List<KafkaEventsGetKafkaEventsResponseDto>> GetAllKafkaEvents(IConfiguration configuration, IKafkaEventsClient kafkaEventsClient)
{
    var apiKeyConfigValue = $"QuickCodeApiKeys:UserManagerModuleApiKey";
    var configApiKey = configuration.GetValue<string>(apiKeyConfigValue);
    SetKafkaApiKeyToClients(kafkaEventsClient, configApiKey!);
    var kafkaEvents = await kafkaEventsClient.GetKafkaEventsAsync();
    return kafkaEvents.ToList();
}
async Task<List<GroupHttpMethodPath>> GetAllGroupMethods(IConfiguration configuration, IApiPermissionGroupsClient apiPermissionGroupsClient, IApiMethodDefinitionsClient apiMethodDefinitionsClient)
{
    
    var apiKeyConfigValue = $"QuickCodeApiKeys:UserManagerModuleApiKey";
    var configApiKey = configuration.GetValue<string>(apiKeyConfigValue);
    SetApiKeyToClients(apiPermissionGroupsClient, apiMethodDefinitionsClient, configApiKey!);
    var authorizationsGroups = await apiPermissionGroupsClient.ApiPermissionGroupsGetAsync();
    var authorizations = await apiMethodDefinitionsClient.ApiMethodDefinitionsGetAsync();
    
    SetApiKeyToClients(apiPermissionGroupsClient, apiMethodDefinitionsClient, "");
    var allMethods = from authGroup in authorizationsGroups
        join a in authorizations on authGroup.ApiMethodDefinitionId equals a.Id
        select new GroupHttpMethodPath()
        {
            PermissionGroupId = authGroup.PermissionGroupId, HttpMethod = a.HttpMethod, Path = a.Path
        };

    return allMethods.ToList();
}

void SetKafkaApiKeyToClients(IKafkaEventsClient kafkaEventsClient, string configUserManagerApiKey)
{
    (kafkaEventsClient as ClientBase)!.SetApiKey(configUserManagerApiKey);
}

void SetApiKeyToClients(IApiPermissionGroupsClient apiPermissionGroupsClient, IApiMethodDefinitionsClient apiMethodDefinitionsClient, string configUserManagerApiKey)
{
    (apiPermissionGroupsClient as ClientBase)!.SetApiKey(configUserManagerApiKey);
    (apiMethodDefinitionsClient as ClientBase)!.SetApiKey(configUserManagerApiKey);
}
