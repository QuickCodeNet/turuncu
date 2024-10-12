using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCode.Turuncu.Common.Nswag;
using QuickCode.Turuncu.Common.Nswag.Clients.UserManagerModuleApi.Contracts;
using QuickCode.Turuncu.Portal.Controllers;

namespace QuickCode.Turuncu.Portal.Helpers
{
     public interface IPortalPermissionManager
    {
        Task<ViewPermission> GetPagePermission(string currentController, string viewName);
    }

    public class PortalPermissionManager : IPortalPermissionManager
    {
        protected IHttpContextAccessor httpContextAccessor;
        private IPortalPermissionsClient portalPermissionsClient;
        private IPortalPermissionGroupsClient portalPermissionGroupsClient;
        private IPortalPermissionTypesClient portalPermissionTypesClient;

        public IHttpContextAccessor GetHttpContextAccessor()
        {
            if (httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated)
            {
                if (httpContextAccessor.HttpContext!.User.Claims.Any(i => i.Type.Equals("QuickCodeApiToken")))
                {
                    var claimAuthToken =
                        httpContextAccessor.HttpContext!.User.Claims.First(i => i.Type.Equals("QuickCodeApiToken"));
       
    
                    ((ClientBase)portalPermissionsClient).SetBearerToken(claimAuthToken!.Value);
                    ((ClientBase)portalPermissionGroupsClient).SetBearerToken(claimAuthToken!.Value);
                    ((ClientBase)portalPermissionTypesClient).SetBearerToken(claimAuthToken!.Value);
                }
            }
            return httpContextAccessor;
        }

         
        public PortalPermissionManager(IPortalPermissionsClient portalPermissionsClient,
            IPortalPermissionGroupsClient portalPermissionGroupsClient,
            IPortalPermissionTypesClient portalPermissionTypesClient, IHttpContextAccessor httpContextAccessor)
        {
            this.portalPermissionsClient = portalPermissionsClient;
            this.portalPermissionGroupsClient = portalPermissionGroupsClient;
            this.portalPermissionTypesClient = portalPermissionTypesClient;
            this.httpContextAccessor = httpContextAccessor;
        }


        private async Task<IEnumerable<SelectListItem>> GetSessionSelectListItem(string key)
        {
            IEnumerable<SelectListItem> returnValue = new List<SelectListItem>();
            if ((returnValue = GetHttpContextAccessor().HttpContext.Session.Get<IEnumerable<SelectListItem>>(key)) == null)
            {
                if (key == "PortalPermissionTypes")
                {
                    returnValue = (await portalPermissionTypesClient.PortalPermissionTypesGetAsync()).AsMultiSelectList("Id", "{0}", "Name");
                }
                else if (key == "PortalPermissions")
                {
                    returnValue = (await portalPermissionsClient.PortalPermissionsGetAsync()).AsMultiSelectList("Id", "{0}", "Name");
                }

                GetHttpContextAccessor().HttpContext.Session.Set(key, returnValue);
            }

            return returnValue;
        }


        private async Task<List<PortalPermissionGroupsDto>> GetPortalPermissionGroup()
        {
            var key = "PortalPermissionGroups";
            List<PortalPermissionGroupsDto> returnValue;
            if ((returnValue =
                    GetHttpContextAccessor().HttpContext!.Session.Get<List<PortalPermissionGroupsDto>>(key)) !=
                null) 
                return returnValue;
            
            var portalPermissionGroups = await portalPermissionGroupsClient.PortalPermissionGroupsGetAsync();
            
            returnValue =
                portalPermissionGroups.Where(i =>
                    i.PermissionGroupId == SessionGroupId).ToList<PortalPermissionGroupsDto>();
            GetHttpContextAccessor().HttpContext!.Session.Set(key, returnValue);

            return returnValue;
        }


        public async Task<ViewPermission> GetPagePermission(string currentController, string viewName)
        {
            var portalPermissionId = 0;
            var authorization = new ViewPermission();
            var portalPermissionResultList = from A in await GetSessionSelectListItem("PortalPermissions")
                                           where A.Text == currentController
                                           select Convert.ToInt32(A.Value);

            var permissionResultList = Enumerable.ToList(portalPermissionResultList);
            if (permissionResultList.Any())
            {
                portalPermissionId = permissionResultList.First();
            }


            var authorizationsGroupInfo = await GetPortalPermissionGroup();
            var result = from A in authorizationsGroupInfo
                         where A.PortalPermissionId == portalPermissionId
                         select A;

            if (result.Any())
            {
                authorization.IsPageAvailable = true;
                foreach (var item in result)
                {
                    string typeName = viewName;
                    var authorizationsList = (from A in await GetSessionSelectListItem("PortalPermissionTypes")
                                              where A.Value == item.PortalPermissionTypeId.ToString()
                                              select A.Text);

                    foreach (var authType in authorizationsList)
                    {
                        authorization.GetType().GetProperty(authType)!.SetValue(authorization, true, null);
                    }
                }
            }

            return authorization;
        }


        public int SessionGroupId
        {
            get
            {
                var groupId = GetHttpContextAccessor().HttpContext.User.Claims.Where(i => i.Type == ClaimTypes.GroupSid).FirstOrDefault().Value;

                return groupId.AsInt32();
            }
        }

    }
}
