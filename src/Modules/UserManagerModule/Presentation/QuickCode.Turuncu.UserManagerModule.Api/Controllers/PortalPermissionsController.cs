using QuickCode.Turuncu.UserManagerModule.Application;
using QuickCode.Turuncu.UserManagerModule.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging; 
using QuickCode.Turuncu.UserManagerModule.Application.Features;
using QuickCode.Turuncu.UserManagerModule.Application.Dtos;
using QuickCode.Turuncu.UserManagerModule.Application.Features.Queries.PortalPermissionGroups;

namespace QuickCode.Turuncu.UserManagerModule.Api.Controllers
{
    public partial class PortalPermissionsController 
    {
	    [HttpGet("get-portal-permissions/{permissionGroupId}")]
        public async Task<Response<PortalPermissionGroupList>> GetPortalPermissions(int permissionGroupId)
        {
            var returnValue = mapper.Map<Response<PortalPermissionGroupList>>(await mediator.Send(new PortalPermissionGroupsGetItemsQuery(permissionGroupId)));
            return returnValue;
        }

        [HttpPost("update-portal-permission")]
		public async Task<Response<bool>> UpdatePortalPermission(UpdatePortalPermissionGroupRequest request)
        {
			if (request.Value == 1)
            {
                var response = await mediator.Send(new PortalPermissionGroupsInsertCommand(new PortalPermissionGroupsDto()
                {
                    PermissionGroupId = request.PermissionGroupId,
                    PortalPermissionId = request.PortalPermissionId,
                    PortalPermissionTypeId = request.PortalPermissionTypeId
                }));
				
				return new Response<bool>() { Value = response.Code == 0 };
            }
            else
            {
                var filterResponse = await mediator.Send(new PortalPermissionGroupsPortalPermissionGroupsGetPortalPermissionGroupQuery(request.PortalPermissionId, request.PermissionGroupId, request.PortalPermissionTypeId));

                if (filterResponse.Value.Any())
                {
                    var deleteItem = filterResponse.Value.First();
                    var deleteResponse = await mediator.Send(new PortalPermissionGroupsDeleteCommand(new PortalPermissionGroupsDto()
                    {
                        PermissionGroupId = deleteItem.PermissionGroupId,
                        PortalPermissionId = deleteItem.PortalPermissionId,
                        PortalPermissionTypeId = deleteItem.PortalPermissionTypeId,
                        Id = deleteItem.Id
                    }));
					
                    return new Response<bool>() { Value = deleteResponse.Value };
                }

            }

            return new Response<bool>() { Value = false };
        }
    }
}

