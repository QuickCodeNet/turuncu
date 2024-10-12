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
    public class PermissionGroupsPermissionGroupsPortalPermissionGroups_KEY_RESTQuery : IRequest<Response<PermissionGroupsPortalPermissionGroups_KEY_RESTResponseDto>>
    {
        public int PermissionGroupsId { get; set; }
        public int PortalPermissionGroupsId { get; set; }

        public PermissionGroupsPermissionGroupsPortalPermissionGroups_KEY_RESTQuery(int permissionGroupsId, int portalPermissionGroupsId)
        {
            this.PermissionGroupsId = permissionGroupsId;
            this.PortalPermissionGroupsId = portalPermissionGroupsId;
        }

        public class PermissionGroupsPermissionGroupsPortalPermissionGroups_KEY_RESTHandler : IRequestHandler<PermissionGroupsPermissionGroupsPortalPermissionGroups_KEY_RESTQuery, Response<PermissionGroupsPortalPermissionGroups_KEY_RESTResponseDto>>
        {
            private readonly ILogger<PermissionGroupsPermissionGroupsPortalPermissionGroups_KEY_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPermissionGroupsRepository _repository;
            public PermissionGroupsPermissionGroupsPortalPermissionGroups_KEY_RESTHandler(IMapper mapper, ILogger<PermissionGroupsPermissionGroupsPortalPermissionGroups_KEY_RESTHandler> logger, IPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<PermissionGroupsPortalPermissionGroups_KEY_RESTResponseDto>> Handle(PermissionGroupsPermissionGroupsPortalPermissionGroups_KEY_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<PermissionGroupsPortalPermissionGroups_KEY_RESTResponseDto>>(await _repository.PermissionGroupsPortalPermissionGroups_KEY_RESTAsync(request.PermissionGroupsId, request.PortalPermissionGroupsId));
                return returnValue;
            }
        }
    }
}