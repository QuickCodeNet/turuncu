using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using QuickCode.Turuncu.Common.Model;

namespace QuickCode.Turuncu.Portal.Helpers;

public class CustomExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionHandlingMiddleware> _logger;
    private readonly IRazorViewEngine _viewEngine;
    private readonly ITempDataProvider _tempDataProvider;
    private readonly IActionContextAccessor _actionContextAccessor;

    public CustomExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<CustomExceptionHandlingMiddleware> logger,
        IRazorViewEngine viewEngine,
        ITempDataProvider tempDataProvider,
        IActionContextAccessor actionContextAccessor)
    {
        _next = next;
        _logger = logger;
        _viewEngine = viewEngine;
        _tempDataProvider = tempDataProvider;
        _actionContextAccessor = actionContextAccessor;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unauthorized access. Status Code: {context.Response.StatusCode}");
            
            var actionContext = _actionContextAccessor.ActionContext ?? new ActionContext
            {
                HttpContext = context,
                RouteData = context.GetRouteData(),
                ActionDescriptor = new ActionDescriptor()
            };

            var statusCode = ex switch
            {
                QuickCodeSwaggerException swaggerException => swaggerException!.StatusCode, 
                _ => (int)HttpStatusCode.InternalServerError
            };

            var viewName = $"Error";
            var viewEngineResult = _viewEngine.FindView(actionContext, viewName, false);
            if (!viewEngineResult.Success)
            {
                throw new InvalidOperationException($"Could not find view: {viewName}");
            }

            context.Response.ContentType = "text/html";

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                ["SessionData"] = context.Session.GetString("MenuItems") // Example
            };
            
            viewData["Layout"] = "_Layout";
            
            var errorCode = statusCode.ToString();
            var errorMessage = "Forbidden";
            var errorDescription = "Undefined Error";

            viewData["ErrorCode"] = errorCode.ToString();
            viewData["ErrorIcon"] = $"ErrorIcon{statusCode}.png";
            
            switch (statusCode)
            {
                case 400:
                    errorMessage = "Bad Request";
                    errorDescription = "The server could not understand the request due to invalid syntax";
                    break;
                case 401:
                    errorMessage = "Unauthorized access";
                    errorDescription = "Your session has expired or you are not authorized to view this page. Please log in again.";
                    break;
                case 403:
                    errorMessage = "Forbidden";
                    errorDescription = "You do not have permission to perform this action.";
                    break;
                case 404:
                    errorMessage = "Page not found";
                    errorDescription = "The page you are looking for could not be found.";
                    break;
                case 500:
                    errorMessage = "Server Error";
                    errorDescription = "Internal Server Error. An unexpected error occurred on the server. Please try again later.";
                    break;
            }

            viewData["ErrorMessage"] = errorMessage;
            viewData["ErrorDescription"] = errorDescription;

            await using var writer = new StringWriter();
            var viewContext = new ViewContext(
                actionContext,
                viewEngineResult.View,
                viewData,
                new TempDataDictionary(context, _tempDataProvider),
                writer,
                new HtmlHelperOptions()
            );

            await viewContext.View.RenderAsync(viewContext);
            var htmlContent = writer.ToString();

            await context.Response.WriteAsync(htmlContent);
        }
    }
}
