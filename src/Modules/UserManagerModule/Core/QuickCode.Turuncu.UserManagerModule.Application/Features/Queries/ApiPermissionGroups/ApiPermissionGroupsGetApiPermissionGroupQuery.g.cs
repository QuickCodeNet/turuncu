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
    public class ApiPermissionGroupsApiPermissionGroupsGetApiPermissionGroupQuery : IRequest<Response<List<ApiPermissionGroupsGetApiPermissionGroupResponseDto>>>
    {
        public int ApiPermissionGroupsPermissionGroupId { get; set; }
        public int ApiPermissionGroupsApiMethodDefinitionId { get; set; }

        public ApiPermissionGroupsApiPermissionGroupsGetApiPermissionGroupQuery(int apiPermissionGroupsPermissionGroupId, int apiPermissionGroupsApiMethodDefinitionId)
        {
            this.ApiPermissionGroupsPermissionGroupId = apiPermissionGroupsPermissionGroupId;
            this.ApiPermissionGroupsApiMethodDefinitionId = apiPermissionGroupsApiMethodDefinitionId;
        }

        public class ApiPermissionGroupsApiPermissionGroupsGetApiPermissionGroupHandler : IRequestHandler<ApiPermissionGroupsApiPermissionGroupsGetApiPermissionGroupQuery, Response<List<ApiPermissionGroupsGetApiPermissionGroupResponseDto>>>
        {
            private readonly ILogger<ApiPermissionGroupsApiPermissionGroupsGetApiPermissionGroupHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IApiPermissionGroupsRepository _repository;
            public ApiPermissionGroupsApiPermissionGroupsGetApiPermissionGroupHandler(IMapper mapper, ILogger<ApiPermissionGroupsApiPermissionGroupsGetApiPermissionGroupHandler> logger, IApiPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<ApiPermissionGroupsGetApiPermissionGroupResponseDto>>> Handle(ApiPermissionGroupsApiPermissionGroupsGetApiPermissionGroupQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<ApiPermissionGroupsGetApiPermissionGroupResponseDto>>>(await _repository.ApiPermissionGroupsGetApiPermissionGroupAsync(request.ApiPermissionGroupsPermissionGroupId, request.ApiPermissionGroupsApiMethodDefinitionId));
                return returnValue;
            }
        }
    }
}