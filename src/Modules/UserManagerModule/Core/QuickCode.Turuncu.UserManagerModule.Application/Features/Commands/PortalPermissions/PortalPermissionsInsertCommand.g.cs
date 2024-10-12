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
    public class PortalPermissionsInsertCommand : IRequest<Response<PortalPermissionsDto>>
    {
        public PortalPermissionsDto request { get; set; }

        public PortalPermissionsInsertCommand(PortalPermissionsDto request)
        {
            this.request = request;
        }

        public class PortalPermissionsInsertHandler : IRequestHandler<PortalPermissionsInsertCommand, Response<PortalPermissionsDto>>
        {
            private readonly ILogger<PortalPermissionsInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalPermissionsRepository _repository;
            public PortalPermissionsInsertHandler(IMapper mapper, ILogger<PortalPermissionsInsertHandler> logger, IPortalPermissionsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<PortalPermissionsDto>> Handle(PortalPermissionsInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<PortalPermissions>(request.request);
                var returnValue = _mapper.Map<Response<PortalPermissionsDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}