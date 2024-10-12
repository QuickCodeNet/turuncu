using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using QuickCode.Turuncu.Portal.Controllers;
using QuickCode.Turuncu.Portal.Helpers;

namespace QuickCode.Turuncu.Portal.TagHelpers
{
    [HtmlTargetElement(Attributes = nameof(IsPageAvailable))]
    public class IsPageAvailableTagHelper : TagHelper
    {
        
        public bool IsPageAvailable { get; set; } = false;
        private List<PropertyInfo> viewAuthorizationProperties { get; set; }
        private readonly IActionContextAccessor actionContext;
        public IPortalPermissionManager portalPermissionManager { get; set; }
        public IsPageAvailableTagHelper(IActionContextAccessor actionContext, IPortalPermissionManager portalPermissionManager)
        {
            this.actionContext = actionContext;
            this.portalPermissionManager = portalPermissionManager;
            this.viewAuthorizationProperties = typeof(ViewPermission).GetProperties().ToList<PropertyInfo>();
        }
		
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var operationKeyRemove = "Item";
            var areaName = actionContext.ActionContext!.RouteData.Values["Area"].AsString();
            var controllerName = actionContext.ActionContext!.RouteData.Values["Controller"].AsString();
            var actionName = actionContext.ActionContext.RouteData.Values["Action"].AsString();
            var operationName = actionName.EndsWith(operationKeyRemove) ? actionName.Substring(0, actionName.IndexOf(operationKeyRemove, StringComparison.Ordinal)) : actionName;
            if(operationName.IsIn("GetModulePermissions", "GetGroupPermissions", "GetKafkaEvents"))
            {
                //TODO: Daha sonra buraya bak
                operationName = "List";
            }
            if (viewAuthorizationProperties.Any(i => i.Name == operationName))
            {
                var result = await portalPermissionManager.GetPagePermission($"{areaName}{controllerName}", actionName);

                var actionIsAvailable = result.GetType().GetProperty(operationName)!.GetValue(result).AsBoolean();
                IsPageAvailable = context.AllAttributes[nameof(IsPageAvailable)].Value.AsString().AsBoolean();
                var isAuthorized = IsPageAvailable;

                if (IsPageAvailable)
                {
                    isAuthorized = actionIsAvailable;
                }
                else
                {
                    isAuthorized = actionIsAvailable == IsPageAvailable;
                }


                if (!isAuthorized)
                {
                    output.SuppressOutput();
                }

            }

        }

    }
}
