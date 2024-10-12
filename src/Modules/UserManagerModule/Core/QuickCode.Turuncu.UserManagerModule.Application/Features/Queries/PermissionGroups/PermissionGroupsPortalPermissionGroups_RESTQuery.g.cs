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
    public class PermissionGroupsPermissionGroupsPortalPermissionGroups_RESTQuery : IRequest<Response<List<PermissionGroupsPortalPermissionGroups_RESTResponseDto>>>
    {
        public int PermissionGroupsId { get; set; }

        public PermissionGroupsPermissionGroupsPortalPermissionGroups_RESTQuery(int permissionGroupsId)
        {
            this.PermissionGroupsId = permissionGroupsId;
        }

        public class PermissionGroupsPermissionGroupsPortalPermissionGroups_RESTHandler : IRequestHandler<PermissionGroupsPermissionGroupsPortalPermissionGroups_RESTQuery, Response<List<PermissionGroupsPortalPermissionGroups_RESTResponseDto>>>
        {
            private readonly ILogger<PermissionGroupsPermissionGroupsPortalPermissionGroups_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPermissionGroupsRepository _repository;
            public PermissionGroupsPermissionGroupsPortalPermissionGroups_RESTHandler(IMapper mapper, ILogger<PermissionGroupsPermissionGroupsPortalPermissionGroups_RESTHandler> logger, IPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<PermissionGroupsPortalPermissionGroups_RESTResponseDto>>> Handle(PermissionGroupsPermissionGroupsPortalPermissionGroups_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<PermissionGroupsPortalPermissionGroups_RESTResponseDto>>>(await _repository.PermissionGroupsPortalPermissionGroups_RESTAsync(request.PermissionGroupsId));
                return returnValue;
            }
        }
    }
}