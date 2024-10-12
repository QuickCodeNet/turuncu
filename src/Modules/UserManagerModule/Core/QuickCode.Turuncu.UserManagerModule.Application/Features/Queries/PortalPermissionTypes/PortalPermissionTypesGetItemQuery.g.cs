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
    public class PortalPermissionTypesGetItemQuery : IRequest<Response<PortalPermissionTypesDto>>
    {
        public int Id { get; set; }

        public PortalPermissionTypesGetItemQuery(int id)
        {
            this.Id = id;
        }

        public class PortalPermissionTypesGetItemHandler : IRequestHandler<PortalPermissionTypesGetItemQuery, Response<PortalPermissionTypesDto>>
        {
            private readonly ILogger<PortalPermissionTypesGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalPermissionTypesRepository _repository;
            public PortalPermissionTypesGetItemHandler(IMapper mapper, ILogger<PortalPermissionTypesGetItemHandler> logger, IPortalPermissionTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<PortalPermissionTypesDto>> Handle(PortalPermissionTypesGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<PortalPermissionTypesDto>>(await _repository.GetByPkAsync(request.Id));
                return returnValue;
            }
        }
    }
}