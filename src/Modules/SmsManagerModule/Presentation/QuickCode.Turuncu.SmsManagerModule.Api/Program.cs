using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Execution;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections;
using QuickCode.Turuncu.SmsManagerModule.Application;
using System.Reflection;
using AutoMapper;
using QuickCode.Turuncu.SmsManagerModule.Persistence.Mappers;
using System.Text;
using QuickCode.Turuncu.SmsManagerModule.Api.Helpers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuickCode.Turuncu.SmsManagerModule.Persistence.Contexts;
using QuickCode.Turuncu.SmsManagerModule.Persistence.Repositories;
using QuickCode.Turuncu.SmsManagerModule.Application.Interfaces.Repositories;
using System.Text.Json.Serialization;
using QuickCode.Turuncu.Common;
using QuickCode.Turuncu.Common.Filters;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using QuickCode.Turuncu.Common.Model;
using QuickCode.Turuncu.Common.Nswag.Extensions;
using Serilog;
    
var builder = WebApplication.CreateBuilder(args);

var useHealthCheck = builder.Configuration.GetSection("AppSettings:UseHealthCheck").Get<bool>();
var databaseType = builder.Configuration.GetSection("AppSettings:DatabaseType").Get<string>();

var readConnectionString = Environment.GetEnvironmentVariable("READ_CONNECTION_STRING");
var writeConnectionString = Environment.GetEnvironmentVariable("WRITE_CONNECTION_STRING");
var elasticConnectionString = Environment.GetEnvironmentVariable("ELASTIC_CONNECTION_STRING");
var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

Log.Information($"Started({environmentName})...");
Log.Information($"ReadConnectionString = {readConnectionString}");
Log.Information($"WriteConnectionString = {writeConnectionString}");

if (!string.IsNullOrEmpty(readConnectionString))
{
    builder.Configuration["ConnectionStrings:ReadConnection"] = readConnectionString;
    Log.Information($"ReadConnection String updated via Environment Variables.");
}

if (!string.IsNullOrEmpty(writeConnectionString))
{
    builder.Configuration["ConnectionStrings:WriteConnection"] = writeConnectionString;
    Log.Information($"WriteConnection String updated via Environment Variables.");
}

if (!string.IsNullOrEmpty(elasticConnectionString))
{
    builder.Configuration["Logging:ElasticConfiguration:Uri"] = elasticConnectionString;
    Log.Information($"Elastic Connection String updated via Environment Variables.");
}

builder.Services.AddLogger(builder.Configuration);
Log.Information($"{builder.Configuration["Logging:ApiName"]} Started.");

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
    cfg.RegisterServicesFromAssembly(typeof(IBaseRepository<>).Assembly);
});

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new ToKebabParameterTransformer(typeof(Program))));
    options.Filters.Add(typeof(ApiLogFilterAttribute));
    options.Filters.Add(new ProducesAttribute("application/json"));
}).AddJsonOptions(jsonOptions => { jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

builder.Services.AddResponseCompression();

builder.Services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });

var config = new AutoMapper.MapperConfiguration(cfg => { cfg.AddProfile(new MappingProfiles()); });

builder.Services.AddSingleton<ApiKeyAuthorizationFilter>();
builder.Services.AddQuickCodeDbContext<ReadDbContext, WriteDbContext>(builder.Configuration);

builder.Services.AddSingleton<ApiKeyAuthorizationFilter>();

//services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddQuickCodeRepositories();
builder.Services.AddQuickCodeSwaggerGen(builder.Configuration);
builder.Services.AddNswagServiceClient(builder.Configuration, typeof(Program));
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

var repositoryInterfaces = typeof(IBaseRepository<>).Assembly.GetTypes()
    .Where(i => i.Name.EndsWith("Repository") && i.IsInterface);
foreach (var interfaceType in repositoryInterfaces)
{
    var implementationType = typeof(WriteDbContext).Assembly.GetTypes().First(i => i.Name == interfaceType.Name[1..]);
    builder.Services.AddScoped(interfaceType, implementationType);
}

var app = builder.Build();
app.UseResponseCompression();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();

// global cors policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

if (useHealthCheck && databaseType != "inMemory")
{
    app.UseHealthChecks("/hc", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHsts();
app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
    var runMigration = Environment.GetEnvironmentVariable("RUN_MIGRATION");
    
    if ((runMigration == null || runMigration!.Equals("yes", StringComparison.CurrentCultureIgnoreCase)) &&
        databaseType != "inMemory")
    {
        await dbContext.Database.MigrateAsync();
    }
}

app.Run();