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
    public class PortalPermissionsPortalPermissionsPortalPermissionGroups_RESTQuery : IRequest<Response<List<PortalPermissionsPortalPermissionGroups_RESTResponseDto>>>
    {
        public int PortalPermissionsId { get; set; }

        public PortalPermissionsPortalPermissionsPortalPermissionGroups_RESTQuery(int portalPermissionsId)
        {
            this.PortalPermissionsId = portalPermissionsId;
        }

        public class PortalPermissionsPortalPermissionsPortalPermissionGroups_RESTHandler : IRequestHandler<PortalPermissionsPortalPermissionsPortalPermissionGroups_RESTQuery, Response<List<PortalPermissionsPortalPermissionGroups_RESTResponseDto>>>
        {
            private readonly ILogger<PortalPermissionsPortalPermissionsPortalPermissionGroups_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalPermissionsRepository _repository;
            public PortalPermissionsPortalPermissionsPortalPermissionGroups_RESTHandler(IMapper mapper, ILogger<PortalPermissionsPortalPermissionsPortalPermissionGroups_RESTHandler> logger, IPortalPermissionsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<PortalPermissionsPortalPermissionGroups_RESTResponseDto>>> Handle(PortalPermissionsPortalPermissionsPortalPermissionGroups_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<PortalPermissionsPortalPermissionGroups_RESTResponseDto>>>(await _repository.PortalPermissionsPortalPermissionGroups_RESTAsync(request.PortalPermissionsId));
                return returnValue;
            }
        }
    }
}