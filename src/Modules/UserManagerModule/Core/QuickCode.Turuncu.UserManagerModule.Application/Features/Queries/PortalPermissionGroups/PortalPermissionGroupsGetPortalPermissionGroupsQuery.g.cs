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
    public class PortalPermissionGroupsPortalPermissionGroupsGetPortalPermissionGroupsQuery : IRequest<Response<List<PortalPermissionGroupsGetPortalPermissionGroupsResponseDto>>>
    {
        public int PortalPermissionGroupsPermissionGroupId { get; set; }

        public PortalPermissionGroupsPortalPermissionGroupsGetPortalPermissionGroupsQuery(int portalPermissionGroupsPermissionGroupId)
        {
            this.PortalPermissionGroupsPermissionGroupId = portalPermissionGroupsPermissionGroupId;
        }

        public class PortalPermissionGroupsPortalPermissionGroupsGetPortalPermissionGroupsHandler : IRequestHandler<PortalPermissionGroupsPortalPermissionGroupsGetPortalPermissionGroupsQuery, Response<List<PortalPermissionGroupsGetPortalPermissionGroupsResponseDto>>>
        {
            private readonly ILogger<PortalPermissionGroupsPortalPermissionGroupsGetPortalPermissionGroupsHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalPermissionGroupsRepository _repository;
            public PortalPermissionGroupsPortalPermissionGroupsGetPortalPermissionGroupsHandler(IMapper mapper, ILogger<PortalPermissionGroupsPortalPermissionGroupsGetPortalPermissionGroupsHandler> logger, IPortalPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<PortalPermissionGroupsGetPortalPermissionGroupsResponseDto>>> Handle(PortalPermissionGroupsPortalPermissionGroupsGetPortalPermissionGroupsQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<PortalPermissionGroupsGetPortalPermissionGroupsResponseDto>>>(await _repository.PortalPermissionGroupsGetPortalPermissionGroupsAsync(request.PortalPermissionGroupsPermissionGroupId));
                return returnValue;
            }
        }
    }
}