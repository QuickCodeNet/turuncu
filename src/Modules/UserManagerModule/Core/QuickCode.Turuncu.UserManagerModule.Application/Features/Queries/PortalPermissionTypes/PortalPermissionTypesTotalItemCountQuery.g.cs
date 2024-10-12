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
    public class PortalPermissionTypesTotalItemCountQuery : IRequest<Response<int>>
    {
        public PortalPermissionTypesTotalItemCountQuery()
        {
        }

        public class PortalPermissionTypesTotalItemCountHandler : IRequestHandler<PortalPermissionTypesTotalItemCountQuery, Response<int>>
        {
            private readonly ILogger<PortalPermissionTypesTotalItemCountHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalPermissionTypesRepository _repository;
            public PortalPermissionTypesTotalItemCountHandler(IMapper mapper, ILogger<PortalPermissionTypesTotalItemCountHandler> logger, IPortalPermissionTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(PortalPermissionTypesTotalItemCountQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<int>>(await _repository.CountAsync());
                return returnValue;
            }
        }
    }
}