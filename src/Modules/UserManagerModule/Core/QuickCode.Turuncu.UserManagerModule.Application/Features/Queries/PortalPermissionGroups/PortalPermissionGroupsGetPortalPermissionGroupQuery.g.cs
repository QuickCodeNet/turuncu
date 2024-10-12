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
    public class PortalPermissionGroupsPortalPermissionGroupsGetPortalPermissionGroupQuery : IRequest<Response<List<PortalPermissionGroupsGetPortalPermissionGroupResponseDto>>>
    {
        public int PortalPermissionGroupsPortalPermissionId { get; set; }
        public int PortalPermissionGroupsPermissionGroupId { get; set; }
        public int PortalPermissionGroupsPortalPermissionTypeId { get; set; }

        public PortalPermissionGroupsPortalPermissionGroupsGetPortalPermissionGroupQuery(int portalPermissionGroupsPortalPermissionId, int portalPermissionGroupsPermissionGroupId, int portalPermissionGroupsPortalPermissionTypeId)
        {
            this.PortalPermissionGroupsPortalPermissionId = portalPermissionGroupsPortalPermissionId;
            this.PortalPermissionGroupsPermissionGroupId = portalPermissionGroupsPermissionGroupId;
            this.PortalPermissionGroupsPortalPermissionTypeId = portalPermissionGroupsPortalPermissionTypeId;
        }

        public class PortalPermissionGroupsPortalPermissionGroupsGetPortalPermissionGroupHandler : IRequestHandler<PortalPermissionGroupsPortalPermissionGroupsGetPortalPermissionGroupQuery, Response<List<PortalPermissionGroupsGetPortalPermissionGroupResponseDto>>>
        {
            private readonly ILogger<PortalPermissionGroupsPortalPermissionGroupsGetPortalPermissionGroupHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalPermissionGroupsRepository _repository;
            public PortalPermissionGroupsPortalPermissionGroupsGetPortalPermissionGroupHandler(IMapper mapper, ILogger<PortalPermissionGroupsPortalPermissionGroupsGetPortalPermissionGroupHandler> logger, IPortalPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<PortalPermissionGroupsGetPortalPermissionGroupResponseDto>>> Handle(PortalPermissionGroupsPortalPermissionGroupsGetPortalPermissionGroupQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<PortalPermissionGroupsGetPortalPermissionGroupResponseDto>>>(await _repository.PortalPermissionGroupsGetPortalPermissionGroupAsync(request.PortalPermissionGroupsPortalPermissionId, request.PortalPermissionGroupsPermissionGroupId, request.PortalPermissionGroupsPortalPermissionTypeId));
                return returnValue;
            }
        }
    }
}