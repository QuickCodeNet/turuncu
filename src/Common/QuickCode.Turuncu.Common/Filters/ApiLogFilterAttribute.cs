using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using QuickCode.Turuncu.Common.Extensions;
using Serilog;
using Serilog.Events;

namespace QuickCode.Turuncu.Common.Filters;

public class ApiLogFilterAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var requestModel = context.ActionArguments;

        var sw = new Stopwatch();
        sw.Start();
        var logsRequest = new QuickCodeApiLog
        {
            ActionName = context.ActionDescriptor.RouteValues["action"],
            ControllerName = context.ActionDescriptor.RouteValues["controller"],
            ClientIp = context.HttpContext.Connection.RemoteIpAddress!.ToString(),
            LogDatetime = DateTime.Now,
            RequestDatetime = DateTime.Now.ToString("s"),
            HostName = context.HttpContext.Request.Path
        };
        
        if (context.HttpContext!.User.Identity!.IsAuthenticated && context.HttpContext!.User.Claims.Any(i => i.Type.Equals("QuickCodeApiToken")))
        {
            var claimAuthToken = context.HttpContext!.User.Claims.First(i => i.Type.Equals("QuickCodeApiToken"));
            context.HttpContext.Request.Headers.Authorization = $"Bearer {claimAuthToken.Value}";
        }
        
        var result = await next();

        if (!logsRequest.ControllerName!.IsInList("Menu"))
        {
            logsRequest.HttpStatusCode = result.HttpContext.Response.StatusCode.ToString();

            logsRequest.ElapsedTime = sw.ElapsedMilliseconds;
            logsRequest.ResponseDatetime = DateTime.Now.ToString("s");

            if (result.Result is ObjectResult)
            {
                var response = result.Result as ObjectResult;
                Log.Information("Api Log Message: {apiLog} {request} {response}", logsRequest.ToJson(),
                    requestModel.ToJson(), response!.Value.ToJson());
            }
            else if (result.Result is ViewResult)
            {
                var response = result.Result as ViewResult;
                Log.Information("Portal Log Message: {apiLog} {request} {response}", logsRequest.ToJson(),
                    requestModel.ToJson(), response!.Model.ToJson());
            }
        }
    }
}

public partial class QuickCodeApiLog
{
    public string? HostIp { get; set; }
    public string? HostName { get; set; }

    public string? ClientIp { get; set; }
    
    public string? ControllerName { get; set; }
    
    public string? ActionName { get; set; }
    
    public string? HttpStatusCode { get; set; }

    public long? ElapsedTime { get; set; }
    
    public string? RequestDatetime { get; set; }
    public string? ResponseDatetime { get; set; }
    
    public DateTime LogDatetime { get; set; }
    
   
    public string? VersionCode { get; set; }
	
}