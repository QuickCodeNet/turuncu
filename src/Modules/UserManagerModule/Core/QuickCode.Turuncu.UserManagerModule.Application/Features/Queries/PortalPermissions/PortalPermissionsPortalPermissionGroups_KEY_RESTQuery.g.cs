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

namespace QuickCode.Turuncu.UserManagerModule.Application.Features
{
    public class PortalPermissionsPortalPermissionsPortalPermissionGroups_KEY_RESTQuery : IRequest<Response<PortalPermissionsPortalPermissionGroups_KEY_RESTResponseDto>>
    {
        public int PortalPermissionsId { get; set; }
        public int PortalPermissionGroupsId { get; set; }

        public PortalPermissionsPortalPermissionsPortalPermissionGroups_KEY_RESTQuery(int portalPermissionsId, int portalPermissionGroupsId)
        {
            this.PortalPermissionsId = portalPermissionsId;
            this.PortalPermissionGroupsId = portalPermissionGroupsId;
        }

        public class PortalPermissionsPortalPermissionsPortalPermissionGroups_KEY_RESTHandler : IRequestHandler<PortalPermissionsPortalPermissionsPortalPermissionGroups_KEY_RESTQuery, Response<PortalPermissionsPortalPermissionGroups_KEY_RESTResponseDto>>
        {
            private readonly ILogger<PortalPermissionsPortalPermissionsPortalPermissionGroups_KEY_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalPermissionsRepository _repository;
            public PortalPermissionsPortalPermissionsPortalPermissionGroups_KEY_RESTHandler(IMapper mapper, ILogger<PortalPermissionsPortalPermissionsPortalPermissionGroups_KEY_RESTHandler> logger, IPortalPermissionsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<PortalPermissionsPortalPermissionGroups_KEY_RESTResponseDto>> Handle(PortalPermissionsPortalPermissionsPortalPermissionGroups_KEY_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<PortalPermissionsPortalPermissionGroups_KEY_RESTResponseDto>>(await _repository.PortalPermissionsPortalPermissionGroups_KEY_RESTAsync(request.PortalPermissionsId, request.PortalPermissionGroupsId));
                return returnValue;
            }
        }
    }
}