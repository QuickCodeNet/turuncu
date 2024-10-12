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
    public class PermissionGroupsPermissionGroupsApiPermissionGroups_KEY_RESTQuery : IRequest<Response<PermissionGroupsApiPermissionGroups_KEY_RESTResponseDto>>
    {
        public int PermissionGroupsId { get; set; }
        public int ApiPermissionGroupsId { get; set; }

        public PermissionGroupsPermissionGroupsApiPermissionGroups_KEY_RESTQuery(int permissionGroupsId, int apiPermissionGroupsId)
        {
            this.PermissionGroupsId = permissionGroupsId;
            this.ApiPermissionGroupsId = apiPermissionGroupsId;
        }

        public class PermissionGroupsPermissionGroupsApiPermissionGroups_KEY_RESTHandler : IRequestHandler<PermissionGroupsPermissionGroupsApiPermissionGroups_KEY_RESTQuery, Response<PermissionGroupsApiPermissionGroups_KEY_RESTResponseDto>>
        {
            private readonly ILogger<PermissionGroupsPermissionGroupsApiPermissionGroups_KEY_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPermissionGroupsRepository _repository;
            public PermissionGroupsPermissionGroupsApiPermissionGroups_KEY_RESTHandler(IMapper mapper, ILogger<PermissionGroupsPermissionGroupsApiPermissionGroups_KEY_RESTHandler> logger, IPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<PermissionGroupsApiPermissionGroups_KEY_RESTResponseDto>> Handle(PermissionGroupsPermissionGroupsApiPermissionGroups_KEY_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<PermissionGroupsApiPermissionGroups_KEY_RESTResponseDto>>(await _repository.PermissionGroupsApiPermissionGroups_KEY_RESTAsync(request.PermissionGroupsId, request.ApiPermissionGroupsId));
                return returnValue;
            }
        }
    }
}