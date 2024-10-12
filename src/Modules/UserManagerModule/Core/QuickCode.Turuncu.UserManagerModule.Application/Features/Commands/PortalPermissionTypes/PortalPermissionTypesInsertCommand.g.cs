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
    public class PortalPermissionTypesInsertCommand : IRequest<Response<PortalPermissionTypesDto>>
    {
        public PortalPermissionTypesDto request { get; set; }

        public PortalPermissionTypesInsertCommand(PortalPermissionTypesDto request)
        {
            this.request = request;
        }

        public class PortalPermissionTypesInsertHandler : IRequestHandler<PortalPermissionTypesInsertCommand, Response<PortalPermissionTypesDto>>
        {
            private readonly ILogger<PortalPermissionTypesInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalPermissionTypesRepository _repository;
            public PortalPermissionTypesInsertHandler(IMapper mapper, ILogger<PortalPermissionTypesInsertHandler> logger, IPortalPermissionTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<PortalPermissionTypesDto>> Handle(PortalPermissionTypesInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<PortalPermissionTypes>(request.request);
                var returnValue = _mapper.Map<Response<PortalPermissionTypesDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}