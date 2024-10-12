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
    public class ApiPermissionGroupsTotalItemCountQuery : IRequest<Response<int>>
    {
        public ApiPermissionGroupsTotalItemCountQuery()
        {
        }

        public class ApiPermissionGroupsTotalItemCountHandler : IRequestHandler<ApiPermissionGroupsTotalItemCountQuery, Response<int>>
        {
            private readonly ILogger<ApiPermissionGroupsTotalItemCountHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IApiPermissionGroupsRepository _repository;
            public ApiPermissionGroupsTotalItemCountHandler(IMapper mapper, ILogger<ApiPermissionGroupsTotalItemCountHandler> logger, IApiPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(ApiPermissionGroupsTotalItemCountQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<int>>(await _repository.CountAsync());
                return returnValue;
            }
        }
    }
}