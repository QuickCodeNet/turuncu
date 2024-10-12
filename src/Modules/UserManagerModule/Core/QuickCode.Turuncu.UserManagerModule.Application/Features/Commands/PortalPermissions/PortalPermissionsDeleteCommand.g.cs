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
    public class PortalPermissionsDeleteCommand : IRequest<Response<bool>>
    {
        public PortalPermissionsDto request { get; set; }

        public PortalPermissionsDeleteCommand(PortalPermissionsDto request)
        {
            this.request = request;
        }

        public class PortalPermissionsDeleteHandler : IRequestHandler<PortalPermissionsDeleteCommand, Response<bool>>
        {
            private readonly ILogger<PortalPermissionsDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalPermissionsRepository _repository;
            public PortalPermissionsDeleteHandler(IMapper mapper, ILogger<PortalPermissionsDeleteHandler> logger, IPortalPermissionsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(PortalPermissionsDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<PortalPermissions>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}