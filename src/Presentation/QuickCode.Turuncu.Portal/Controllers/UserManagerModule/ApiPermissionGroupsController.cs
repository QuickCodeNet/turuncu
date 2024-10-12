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
    [Permission("UserManagerModuleApiPermissionGroups")]
    public partial class ApiPermissionGroupsController : BaseController
    {
        [Route("GetModulePermissions")]
        [HttpGet]
        public async Task<IActionResult> GetModulePermissions()
        {
            var model = GetModel<GetApiPermissionGroupData>();
            var groups = await pagePermissionGroupsClient.PermissionGroupsGetAsync();
            model.SelectedGroupId = groups.First().Id;
            model.ComboList = await FillPageComboBoxes(model.ComboList);
            model.Items = (await pageApiMethodDefinitionsClient.GetApiPermissionsAsync(model.SelectedGroupId)).Value;
            SetModelBinder(ref model);
            return View("ApiPermissionGroups", model);
        }

        [Route("GetModulePermissions")]
        [HttpPost]
        public async Task<IActionResult> GetModulePermissions(GetApiPermissionGroupData model)
        {
            ModelBinder(ref model);
            model.Items = (await pageApiMethodDefinitionsClient.GetApiPermissionsAsync(model.SelectedGroupId)).Value;
            SetModelBinder(ref model);
            return View("ApiPermissionGroups", model);
        }

        [Route("UpdatePermission")]
        [HttpPost]
        public async Task<JsonResult> UpdatePermission(UpdateGroupAuthorizationApiRequestData model)
        {
            var result = await pageApiMethodDefinitionsClient.UpdateApiPermissionAsync(model);
            httpContextAccessor.HttpContext!.Session.Remove("PortalPermissions");
            httpContextAccessor.HttpContext!.Session.Remove("ApiPermissions");
            httpContextAccessor.HttpContext!.Session.Remove("PortalPermissionGroups");
            httpContextAccessor.HttpContext!.Session.Remove("ApiPermissionGroups");
            httpContextAccessor.HttpContext!.Session.Remove("PortalPermissionTypes");
            httpContextAccessor.HttpContext!.Session.Remove("MenuItems");
            return Json(result);
        }

    }
}

