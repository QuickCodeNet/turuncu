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
    public class PortalPermissionGroupsDeleteCommand : IRequest<Response<bool>>
    {
        public PortalPermissionGroupsDto request { get; set; }

        public PortalPermissionGroupsDeleteCommand(PortalPermissionGroupsDto request)
        {
            this.request = request;
        }

        public class PortalPermissionGroupsDeleteHandler : IRequestHandler<PortalPermissionGroupsDeleteCommand, Response<bool>>
        {
            private readonly ILogger<PortalPermissionGroupsDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalPermissionGroupsRepository _repository;
            public PortalPermissionGroupsDeleteHandler(IMapper mapper, ILogger<PortalPermissionGroupsDeleteHandler> logger, IPortalPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(PortalPermissionGroupsDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<PortalPermissionGroups>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}