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
    public class ApiPermissionGroupsListQuery : IRequest<Response<List<ApiPermissionGroupsDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public ApiPermissionGroupsListQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class ApiPermissionGroupsListHandler : IRequestHandler<ApiPermissionGroupsListQuery, Response<List<ApiPermissionGroupsDto>>>
        {
            private readonly ILogger<ApiPermissionGroupsListHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IApiPermissionGroupsRepository _repository;
            public ApiPermissionGroupsListHandler(IMapper mapper, ILogger<ApiPermissionGroupsListHandler> logger, IApiPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<ApiPermissionGroupsDto>>> Handle(ApiPermissionGroupsListQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<ApiPermissionGroupsDto>>>(await _repository.ListAsync(request.PageNumber, request.PageSize));
                return returnValue;
            }
        }
    }
}