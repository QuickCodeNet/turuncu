using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuickCode.Turuncu.Portal.Helpers;
using QuickCode.Turuncu.Portal.Models;

namespace QuickCode.Turuncu.Portal.ViewComponents
{
    public class OperationButtons : ViewComponent
    {
        public IPortalPermissionManager portalPermissionManager { get; set; }
        public OperationButtons(IPortalPermissionManager portalPermissionManager)
        {
            this.portalPermissionManager = portalPermissionManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string itemId, string controllerName, string actionName)
        {
            var areaName = ViewContext.RouteData.Values["Area"]!.ToString();
            if (actionName.AsString().Trim().Length == 0)
            {
                actionName = ViewContext.RouteData.Values["Action"]!.ToString();
            }
            if (controllerName.AsString().Trim().Length == 0)
            {
                controllerName = ViewContext.RouteData.Values["Controller"]!.ToString();
            }

            var result = await portalPermissionManager.GetPagePermission($"{areaName}{controllerName}", actionName);

            ViewPermissionItemData model = new ViewPermissionItemData
            {
                Item = result,
                ItemId = itemId,
                ControllerName = controllerName!.Replace("Controller", string.Empty),
                ActionName = actionName
            };
            
            return View(model);
        }
    }
}
