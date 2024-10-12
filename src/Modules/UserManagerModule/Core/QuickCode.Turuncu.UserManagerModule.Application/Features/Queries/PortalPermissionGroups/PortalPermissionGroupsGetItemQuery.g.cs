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
    public class PortalPermissionGroupsGetItemQuery : IRequest<Response<PortalPermissionGroupsDto>>
    {
        public int Id { get; set; }

        public PortalPermissionGroupsGetItemQuery(int id)
        {
            this.Id = id;
        }

        public class PortalPermissionGroupsGetItemHandler : IRequestHandler<PortalPermissionGroupsGetItemQuery, Response<PortalPermissionGroupsDto>>
        {
            private readonly ILogger<PortalPermissionGroupsGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalPermissionGroupsRepository _repository;
            public PortalPermissionGroupsGetItemHandler(IMapper mapper, ILogger<PortalPermissionGroupsGetItemHandler> logger, IPortalPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<PortalPermissionGroupsDto>> Handle(PortalPermissionGroupsGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<PortalPermissionGroupsDto>>(await _repository.GetByPkAsync(request.Id));
                return returnValue;
            }
        }
    }
}