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
    public class PortalPermissionTypesDeleteCommand : IRequest<Response<bool>>
    {
        public PortalPermissionTypesDto request { get; set; }

        public PortalPermissionTypesDeleteCommand(PortalPermissionTypesDto request)
        {
            this.request = request;
        }

        public class PortalPermissionTypesDeleteHandler : IRequestHandler<PortalPermissionTypesDeleteCommand, Response<bool>>
        {
            private readonly ILogger<PortalPermissionTypesDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalPermissionTypesRepository _repository;
            public PortalPermissionTypesDeleteHandler(IMapper mapper, ILogger<PortalPermissionTypesDeleteHandler> logger, IPortalPermissionTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(PortalPermissionTypesDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<PortalPermissionTypes>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}