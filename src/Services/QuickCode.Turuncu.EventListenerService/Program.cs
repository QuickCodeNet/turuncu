using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using QuickCode.Turuncu.Common;
using QuickCode.Turuncu.Common.Controllers;
using QuickCode.Turuncu.Common.Nswag.Extensions;
using QuickCode.Turuncu.EventListenerService;
using Serilog;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

var builder = WebApplication.CreateBuilder(args);
string yamlContent = @"
name: 'Order Processing Workflow'
version: '1.0.0'
description: 'A workflow for processing customer orders'

variables:
  retryCount:
    type: 'int'
    value: '3'
  apiBaseUrl:
    type: 'string'
    value: 'https://api.example.com'
  timeoutDuration:
    type: 'int'
    value: '30'

steps:
  validateOrder:
    url: '{{QuickCodeClients.QuickCodeModuleApi}}/api/quick-code-module/db-types'
    method: 'GET'
    headers:
      X-Api-Key: '{{QuickCodeApiKeys.QuickCodeModuleApiKey}}'
    body:
    timeoutSeconds: 30
    onSuccess:
      - condition: 'validateOrder.statusCode == 404'
        action: 'processPayment'
      - condition: 'response.isValid == true'
        action: 'processPayment'
      - condition: 'default'
        action: 'handleInvalidOrder'
    steps:
      processPayment:
        url: '{{variables.apiBaseUrl}}/payments'
        method: 'POST'
        headers:
          Authorization: 'Bearer {{input.apiKey}}'
        body:
          orderId: '{{input.orderId}}'
          amount: '{{validateOrder.response.totalAmount}}'
      handleInvalidOrder:
        url: '{{variables.apiBaseUrl}}/reject'
        method: 'POST'
        headers:
          Authorization: 'Bearer {{input.apiKey}}'
        body:
          orderId: '{{input.orderId}}'
          reason: '{{validateOrder.response.errorMessage}}'

  checkInventory:
    url: '{{variables.apiBaseUrl}}/inventory/check'
    method: 'GET'
    headers:
      Authorization: 'Bearer {{input.apiKey}}'
    dependsOn:
      - 'validateOrder'
    condition: 'validateOrder.response.isValid == true'
    repeat: '{{variables.retryCount}}'

  createShipment:
    url: '{{variables.apiBaseUrl}}/shipments'
    method: 'POST'
    headers:
      Authorization: 'Bearer {{input.apiKey}}'
    body:
      orderId: '{{input.orderId}}'
      address: '{{validateOrder.response.shippingAddress}}'
    dependsOn:
      - 'checkInventory'
    timeoutSeconds: 30";

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
Log.Information($"Started({environmentName})...");

var useHealthCheck = builder.Configuration.GetSection("AppSettings:UseHealthCheck").Get<bool>();
var elasticConnectionString = Environment.GetEnvironmentVariable("ELASTIC_CONNECTION_STRING");
var kafkaBootstrapServers = builder.Configuration.GetValue<string>("Kafka:BootstrapServers");

if (!string.IsNullOrEmpty(elasticConnectionString))
{
    builder.Configuration["Logging:ElasticConfiguration:Uri"] = elasticConnectionString;
    Log.Information($"Elastic Connection String updated via Environment Variables.");
}

builder.Services.AddLogger(builder.Configuration);
Log.Information($"{builder.Configuration["Logging:ApiName"]} Started.");

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddNswagServiceClient(builder.Configuration, typeof(QuickCodeBaseApiController));

builder.Services.AddHostedService<DynamicKafkaBackgroundService>();
builder.Services.AddControllers();
builder.Services.AddHealthChecks()
    .AddCheck("kafka", new KafkaHealthCheck(kafkaBootstrapServers!));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (useHealthCheck)
{
    app.UseHealthChecks("/hc", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/", () => "Kafka Background Event Listener Service is running...")
    .WithOpenApi();

app.MapPost("/set-topic-refresh-interval", ( [FromBody] int seconds) => 
    {
        if (seconds > 30)
        {
            DynamicKafkaBackgroundService.SetTopicRefreshInterval(seconds);
        }

        return $"Topic refresh interval set to {DynamicKafkaBackgroundService.GetTopicRefreshInterval()} seconds";
    })
    .WithOpenApi();

app.MapPost("/set-topic-listener-interval", ( [FromBody] int seconds) => 
    {
        if (seconds > 30)
        {
            DynamicKafkaBackgroundService.SetTopicListenerInterval(seconds);
        }
 
        return $"Topic listen interval set to {DynamicKafkaBackgroundService.GetTopicListenerInterval()} seconds";
    })
    .WithOpenApi();

app.MapGet("/get-topic-refresh-interval", () => DynamicKafkaBackgroundService.GetTopicRefreshInterval())
    .WithOpenApi();

app.MapGet("/get-topic-listener-interval", () => DynamicKafkaBackgroundService.GetTopicListenerInterval())
    .WithOpenApi();


app.MapPost("/execute-workflow", async (HttpContext context, IHttpClientFactory httpClientFactory,
    IConfiguration configuration, ILogger<Program> logger) =>
{
    try
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var workflow = deserializer.Deserialize<Workflow>(yamlContent);

        var executor = new WorkflowExecutor(httpClientFactory.CreateClient(), context.Request.Query, logger,
            configuration);
        var results = await executor.ExecuteWorkflow(workflow);

        return Results.Ok(results);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Workflow execution failed");
        return Results.Problem("Workflow execution failed", statusCode: 500);
    }
});

app.Run();
