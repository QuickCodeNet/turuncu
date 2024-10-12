using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using QuickCode.Turuncu.Common;
using QuickCode.Turuncu.Common.Controllers;
using QuickCode.Turuncu.Common.Nswag.Extensions;
using QuickCode.Turuncu.EventListenerService;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
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

app.Run();
