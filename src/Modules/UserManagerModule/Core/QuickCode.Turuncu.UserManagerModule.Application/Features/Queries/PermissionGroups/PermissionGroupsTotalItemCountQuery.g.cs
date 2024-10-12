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
    public class PermissionGroupsTotalItemCountQuery : IRequest<Response<int>>
    {
        public PermissionGroupsTotalItemCountQuery()
        {
        }

        public class PermissionGroupsTotalItemCountHandler : IRequestHandler<PermissionGroupsTotalItemCountQuery, Response<int>>
        {
            private readonly ILogger<PermissionGroupsTotalItemCountHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPermissionGroupsRepository _repository;
            public PermissionGroupsTotalItemCountHandler(IMapper mapper, ILogger<PermissionGroupsTotalItemCountHandler> logger, IPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(PermissionGroupsTotalItemCountQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<int>>(await _repository.CountAsync());
                return returnValue;
            }
        }
    }
}