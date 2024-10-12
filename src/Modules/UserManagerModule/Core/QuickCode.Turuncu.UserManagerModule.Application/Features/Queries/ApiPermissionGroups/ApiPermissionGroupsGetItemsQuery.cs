using AutoMapper;
using System.Linq;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.Turuncu.UserManagerModule.Application.Dtos;
using QuickCode.Turuncu.UserManagerModule.Application.Models;
using QuickCode.Turuncu.UserManagerModule.Application.Interfaces.Repositories;
using QuickCode.Turuncu.Common.Helpers;

namespace QuickCode.Turuncu.UserManagerModule.Api.Application.Features.Queries.ApiPermissionGroups
{
    public class ApiPermissionGroupsGetItemsQuery : IRequest<Response<ApiModulePermissions>>
    {
        public int permissionGroupId { get; set; }

        public ApiPermissionGroupsGetItemsQuery(int permissionGroupId)
        {
            this.permissionGroupId = permissionGroupId;
        }

        public class AuthorizationsApiGroupsGetItemsHandler : IRequestHandler<ApiPermissionGroupsGetItemsQuery, Response<ApiModulePermissions>>
        {
            private readonly ILogger<AuthorizationsApiGroupsGetItemsHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IApiPermissionGroupsRepository _apiPermissionGroupsRepository;
            private readonly IApiMethodDefinitionsRepository _apiMethodDefinitionsRepository;
            
            public AuthorizationsApiGroupsGetItemsHandler(IMapper mapper, ILogger<AuthorizationsApiGroupsGetItemsHandler> logger, 
                IApiPermissionGroupsRepository apiPermissionGroupsRepository,
                IApiMethodDefinitionsRepository apiPermissionsRepositor)
            {
                _mapper = mapper;
                _logger = logger;
                _apiPermissionGroupsRepository = apiPermissionGroupsRepository;
                _apiMethodDefinitionsRepository = apiPermissionsRepositor;
            }

            public async Task<Response<ApiModulePermissions>> Handle(ApiPermissionGroupsGetItemsQuery request, CancellationToken cancellationToken)
            {
                var returnValue = new Response<ApiModulePermissions>
                {
                    Value = new ApiModulePermissions
                    {
                        PermissionGroupId = request.permissionGroupId,
                        ApiModulePermissionList = []
                    }
                };
                
                var authorizationsApis = (await _apiMethodDefinitionsRepository.ListAsync()).Value.OrderBy(i => i.Path)
                    .ThenBy(i => i.ControllerName);
                
                var authorizationApiGroupData = (await _apiPermissionGroupsRepository.ApiPermissionGroupsGetApiPermissionGroupsAsync(request.permissionGroupId)).Value;
                foreach (var authorizationApi in authorizationsApis.OrderBy(i => i.ControllerName)
                             .ThenBy(i => i.HttpMethod))
                {

                    var isExists = authorizationApiGroupData.Exists(i =>
                        i.PermissionGroupId.Equals(request.permissionGroupId) && i.ApiMethodDefinitionId.Equals(authorizationApi.Id));

                    var item = new ApiMethodDefinitionItem
                    {
                        Id = authorizationApi.Id,
                        ControllerName = authorizationApi.ControllerName,
                        HttpMethod = authorizationApi.HttpMethod,
                        Path = authorizationApi.Path,
                        ItemType = authorizationApi.ItemType,
                        Value = isExists
                    };

                    var controllerItems = new Dictionary<string, List<ApiMethodDefinitionItem>>();
                    var itemList = new List<ApiMethodDefinitionItem>();
                    var moduleName = item.Path.Split('/')[2].KebabCaseToPascal();
                    if (item.ControllerName.Equals("AuthenticationController"))
                    {
                        continue;
                    }
                    var controllerName = item.ControllerName[0..^"Controller".Length].PascalToKebabCase().KebabCaseToPascal();
                    returnValue.Value.ApiModulePermissionList.TryAdd(moduleName, controllerItems);
                    controllerItems = returnValue.Value.ApiModulePermissionList[moduleName];
                    controllerItems.TryAdd(controllerName, itemList);
                    returnValue.Value.ApiModulePermissionList[moduleName][controllerName].Add(item);
                }

                return returnValue;
            }
        }
    }
}
