using System;
using System.Collections.Generic;
using System.Linq;
using QuickCode.Turuncu.Portal.Models;
using QuickCode.Turuncu.Portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using QuickCode.Turuncu.Portal.Helpers.Authorization;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;

namespace QuickCode.Turuncu.Portal.Controllers.UserManagerModule
{
    [Permission("UserManagerModulePortalPermissions")]
    public partial class PortalPermissionGroupsController : BaseController
    {
        [Route("GetGroupPermissions")]
        [HttpGet]
        public async Task<IActionResult> GetGroupPermissions()
        {
            var model = GetModel<GetPortalPermissionGroupData>();
            var groups = await pagePermissionGroupsClient.PermissionGroupsGetAsync();
            model.SelectedGroupId = groups.First().Id;
            model.ComboList = await FillPageComboBoxes(model.ComboList);
            model.Items = (await pagePortalPermissionsClient.GetPortalPermissionsAsync(model.SelectedGroupId)).Value;
            SetModelBinder(ref model);
            return View("PortalPermissionGroups", model);
        }

        [Route("GetGroupPermissions")]
        [HttpPost]
        public async Task<IActionResult> GetGroupPermissions(GetPortalPermissionGroupData model)
        {
            ModelBinder(ref model);
            model.Items = (await pagePortalPermissionsClient.GetPortalPermissionsAsync(model.SelectedGroupId)).Value;
            SetModelBinder(ref model);
            return View("PortalPermissionGroups", model);
        }

        [Route("UpdatePermission")]
        [HttpPost]
        public async Task<JsonResult> UpdatePermission(UpdateGroupAuthorizationRequestData model)
        {
            var result = await pagePortalPermissionsClient.UpdatePortalPermissionAsync(model);
            httpContextAccessor.HttpContext!.Session.Remove("PortalPermissions");
            httpContextAccessor.HttpContext!.Session.Remove("PortalPermissionGroups");
            httpContextAccessor.HttpContext!.Session.Remove("PortalPermissionTypes");
            httpContextAccessor.HttpContext!.Session.Remove("MenuItems");
            return Json(result);
        }

    }
}

