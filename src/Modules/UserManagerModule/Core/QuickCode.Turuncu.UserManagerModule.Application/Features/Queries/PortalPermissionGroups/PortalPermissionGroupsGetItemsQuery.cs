using AutoMapper;
using System.Linq;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.Turuncu.UserManagerModule.Application.Models;
using QuickCode.Turuncu.UserManagerModule.Domain.Entities;
using QuickCode.Turuncu.UserManagerModule.Application.Interfaces.Repositories;
using QuickCode.Turuncu.UserManagerModule.Application.Dtos;

namespace QuickCode.Turuncu.UserManagerModule.Application.Features.Queries.PortalPermissionGroups
{
    public class PortalPermissionGroupsGetItemsQuery : IRequest<Response<PortalPermissionGroupList>>
    {
        private int permissionGroupId { get; set; }

        public PortalPermissionGroupsGetItemsQuery(int permissionGroupId)
        {
            this.permissionGroupId = permissionGroupId;
        }

        public class PortalPermissionGroupsGetItemsHandler : IRequestHandler<PortalPermissionGroupsGetItemsQuery, Response<PortalPermissionGroupList>>
        {
            private readonly ILogger<PortalPermissionGroupsGetItemsHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalPermissionGroupsRepository _portalPermissionGroupsRepository;
            private readonly IPortalPermissionTypesRepository _portalPermissionTypesRepository;
            private readonly IPortalPermissionsRepository _portalPermissionsRepository;
            
            public PortalPermissionGroupsGetItemsHandler(IMapper mapper, ILogger<PortalPermissionGroupsGetItemsHandler> logger, 
                IPortalPermissionGroupsRepository portalPermissionGroupsRepository,
                IPortalPermissionTypesRepository portalPermissionTypesRepository,
                IPortalPermissionsRepository portalPermissionsRepository)
            {
                _mapper = mapper;
                _logger = logger;
                _portalPermissionGroupsRepository = portalPermissionGroupsRepository;
                _portalPermissionTypesRepository = portalPermissionTypesRepository;
                _portalPermissionsRepository= portalPermissionsRepository;
            }

            public async Task<Response<PortalPermissionGroupList>> Handle(PortalPermissionGroupsGetItemsQuery request, CancellationToken cancellationToken)
            {
                var returnValue = new Response<PortalPermissionGroupList>
                {
                    Value = new PortalPermissionGroupList
                    {
                        PermissionGroupId = request.permissionGroupId,
                        PortalPermissions = []
                    }
                };

                var permissionTypes = (await _portalPermissionTypesRepository.ListAsync()).Value;
                var permissions = (await _portalPermissionsRepository.ListAsync()).Value;
                var permissionGroupData = (await _portalPermissionGroupsRepository.PortalPermissionGroupsGetPortalPermissionGroupsAsync(request.permissionGroupId)).Value;
                foreach (var portalPermission in permissions.OrderBy(i=>i.Name))
                {
                    var item = new PortalPermissionItem()
                    {
                        PortalPermssionId = portalPermission.Id,
                        PortalPermissionTypes = []
                    };

                    foreach (var type in permissionTypes)
                    {
                        var typeItem = new PortalPermissionTypeItem()
                        {
                            PortalPermssionTypeId = type.Id
                        };
                        
                        var result = permissionGroupData!.Where(i =>
                            i.PortalPermissionId == portalPermission.Id && i.PortalPermissionTypeId == type.Id);
                        typeItem.Value = result.Any();
                        item.PortalPermissionTypes.Add(typeItem);
                    }

                    returnValue.Value.PortalPermissions.Add(item);
                }

                return returnValue;
            }
        }
    }
}
