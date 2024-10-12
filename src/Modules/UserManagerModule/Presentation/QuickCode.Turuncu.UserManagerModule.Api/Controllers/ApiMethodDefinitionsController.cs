using QuickCode.Turuncu.UserManagerModule.Application.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuickCode.Turuncu.UserManagerModule.Api.Application.Features.Queries.ApiPermissionGroups;
using QuickCode.Turuncu.UserManagerModule.Application.Features;
using QuickCode.Turuncu.UserManagerModule.Application.Dtos;

namespace QuickCode.Turuncu.UserManagerModule.Api.Controllers
{
    public partial class ApiMethodDefinitionsController 
    {
	    [HttpGet("get-api-permissions/{permissionGroupId}")]
        public async Task<Response<ApiModulePermissions>> GetApiPermissions(int permissionGroupId)
        {
            var returnValue = mapper.Map<Response<ApiModulePermissions>>(await mediator.Send(new ApiPermissionGroupsGetItemsQuery(permissionGroupId)));
            return returnValue;
        }

        [HttpPost("update-api-permission")]
		public async Task<Response<bool>> UpdateApiPermission(UpdateApiPermissionGroupRequest request)
        {
			if (request.Value == 1)
            {
                var response = await mediator.Send(new ApiPermissionGroupsInsertCommand(new ApiPermissionGroupsDto()
                {
                    PermissionGroupId = request.PermissionGroupId,
                    ApiMethodDefinitionId = request.ApiMethodDefinitionId
                }));
				
				return new Response<bool>() { Value = response.Code == 0 };
            }
            else
            {
                var filterResponse = await mediator.Send(new ApiPermissionGroupsApiPermissionGroupsGetApiPermissionGroupQuery(request.PermissionGroupId, request.ApiMethodDefinitionId));

                if (filterResponse.Value.Any())
                {
                    var deleteItem = filterResponse.Value.First();
                    var deleteResponse = await mediator.Send(new ApiPermissionGroupsDeleteCommand(new ApiPermissionGroupsDto()
                    {
                        PermissionGroupId =  deleteItem.PermissionGroupId,
                        ApiMethodDefinitionId = deleteItem.ApiMethodDefinitionId,
                        Id = deleteItem.Id
                    }));
					
                    return new Response<bool>() { Value = deleteResponse.Value };
                }
            }

            return new Response<bool>() { Value = false };
        }
    }
}

