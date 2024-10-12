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
    public class PortalPermissionGroupsTotalItemCountQuery : IRequest<Response<int>>
    {
        public PortalPermissionGroupsTotalItemCountQuery()
        {
        }

        public class PortalPermissionGroupsTotalItemCountHandler : IRequestHandler<PortalPermissionGroupsTotalItemCountQuery, Response<int>>
        {
            private readonly ILogger<PortalPermissionGroupsTotalItemCountHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalPermissionGroupsRepository _repository;
            public PortalPermissionGroupsTotalItemCountHandler(IMapper mapper, ILogger<PortalPermissionGroupsTotalItemCountHandler> logger, IPortalPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(PortalPermissionGroupsTotalItemCountQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<int>>(await _repository.CountAsync());
                return returnValue;
            }
        }
    }
}