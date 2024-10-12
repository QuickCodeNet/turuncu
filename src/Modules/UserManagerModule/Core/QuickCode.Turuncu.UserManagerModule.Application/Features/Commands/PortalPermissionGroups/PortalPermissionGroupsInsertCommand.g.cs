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
    public class PortalPermissionGroupsInsertCommand : IRequest<Response<PortalPermissionGroupsDto>>
    {
        public PortalPermissionGroupsDto request { get; set; }

        public PortalPermissionGroupsInsertCommand(PortalPermissionGroupsDto request)
        {
            this.request = request;
        }

        public class PortalPermissionGroupsInsertHandler : IRequestHandler<PortalPermissionGroupsInsertCommand, Response<PortalPermissionGroupsDto>>
        {
            private readonly ILogger<PortalPermissionGroupsInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalPermissionGroupsRepository _repository;
            public PortalPermissionGroupsInsertHandler(IMapper mapper, ILogger<PortalPermissionGroupsInsertHandler> logger, IPortalPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<PortalPermissionGroupsDto>> Handle(PortalPermissionGroupsInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<PortalPermissionGroups>(request.request);
                var returnValue = _mapper.Map<Response<PortalPermissionGroupsDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}