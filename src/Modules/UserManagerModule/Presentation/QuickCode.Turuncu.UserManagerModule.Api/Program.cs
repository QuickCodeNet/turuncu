using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using QuickCode.Turuncu.UserManagerModule.Api.Helpers;
using QuickCode.Turuncu.UserManagerModule.Persistence.Mappers;
using QuickCode.Turuncu.UserManagerModule.Persistence.Contexts;
using QuickCode.Turuncu.UserManagerModule.Application.Interfaces.Repositories;
using QuickCode.Turuncu.Common;
using QuickCode.Turuncu.Common.Extensions;
using QuickCode.Turuncu.Common.Filters;
using QuickCode.Turuncu.Common.Model;
using QuickCode.Turuncu.Common.Nswag.Extensions;
using QuickCode.Turuncu.UserManagerModule.Api.Extension;

var builder = WebApplication.CreateBuilder(args);

var readConnectionString = Environment.GetEnvironmentVariable("READ_CONNECTION_STRING");
var writeConnectionString = Environment.GetEnvironmentVariable("WRITE_CONNECTION_STRING");
var elasticConnectionString = Environment.GetEnvironmentVariable("ELASTIC_CONNECTION_STRING");
var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
builder.WebHost.UseUrls($"http://*:{port}");

Log.Information($"Started({environmentName} Port:{port})...");
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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
    options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
})
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        var secretKey = Environment.GetEnvironmentVariable("QUICKCODE_JWT_SECRET_KEY"); 
        var securityKey = Encoding.UTF8.GetBytes(secretKey!);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(securityKey)
        };
    })
    .AddCookie(IdentityConstants.ApplicationScheme, options =>
    {
        options.Cookie.Name = "QuickCodeCookieName";
        options.LoginPath = "/api/auth/login";
        options.LogoutPath = "/api/auth/logout";
        options.AccessDeniedPath = "/api/auth/access-denied";
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new ToKebabParameterTransformer(typeof(Program))));
    options.Filters.Add(typeof(ApiLogFilterAttribute));
    options.Filters.Add(new ProducesAttribute("application/json"));
}).AddJsonOptions(jsonOptions => { jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddSingleton<SoftDeleteInterceptor>();
builder.Services.AddQuickCodeIdentityDbContext<AppDbContext>(builder.Configuration);
builder.Services.AddIdentityCore<ApiUser>().AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddClaimsPrincipalFactory<CustomClaimsPrincipalFactory>()
    .AddApiEndpoints();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});

builder.Services.AddResponseCompression();

builder.Services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });

var config = new AutoMapper.MapperConfiguration(cfg => { cfg.AddProfile(new MappingProfiles()); });

builder.Services.AddSingleton<ApiKeyAuthorizationFilter>();
builder.Services.AddQuickCodeDbContext<ReadDbContext, WriteDbContext>(builder.Configuration);

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
builder.Services.AddEndpointsApiExplorer();
var useHealthCheck = builder.Configuration.GetSection("AppSettings:UseHealthCheck").Get<bool>();
var databaseType = builder.Configuration.GetSection("AppSettings:DatabaseType").Get<string>();
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

app.MapGroup("/api/auth").WithTags("Authentications").MapMyIdentityApi<ApiUser>();

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
        var originalCulture = CultureInfo.CurrentCulture;
        var originalUICulture = CultureInfo.CurrentUICulture;

        try
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

            await dbContext.Database.MigrateAsync();
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUICulture;
        }
    }
}

app.Run();