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
    public class PortalPermissionTypesPortalPermissionTypesPortalPermissionGroups_KEY_RESTQuery : IRequest<Response<PortalPermissionTypesPortalPermissionGroups_KEY_RESTResponseDto>>
    {
        public int PortalPermissionTypesId { get; set; }
        public int PortalPermissionGroupsId { get; set; }

        public PortalPermissionTypesPortalPermissionTypesPortalPermissionGroups_KEY_RESTQuery(int portalPermissionTypesId, int portalPermissionGroupsId)
        {
            this.PortalPermissionTypesId = portalPermissionTypesId;
            this.PortalPermissionGroupsId = portalPermissionGroupsId;
        }

        public class PortalPermissionTypesPortalPermissionTypesPortalPermissionGroups_KEY_RESTHandler : IRequestHandler<PortalPermissionTypesPortalPermissionTypesPortalPermissionGroups_KEY_RESTQuery, Response<PortalPermissionTypesPortalPermissionGroups_KEY_RESTResponseDto>>
        {
            private readonly ILogger<PortalPermissionTypesPortalPermissionTypesPortalPermissionGroups_KEY_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalPermissionTypesRepository _repository;
            public PortalPermissionTypesPortalPermissionTypesPortalPermissionGroups_KEY_RESTHandler(IMapper mapper, ILogger<PortalPermissionTypesPortalPermissionTypesPortalPermissionGroups_KEY_RESTHandler> logger, IPortalPermissionTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<PortalPermissionTypesPortalPermissionGroups_KEY_RESTResponseDto>> Handle(PortalPermissionTypesPortalPermissionTypesPortalPermissionGroups_KEY_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<PortalPermissionTypesPortalPermissionGroups_KEY_RESTResponseDto>>(await _repository.PortalPermissionTypesPortalPermissionGroups_KEY_RESTAsync(request.PortalPermissionTypesId, request.PortalPermissionGroupsId));
                return returnValue;
            }
        }
    }
}