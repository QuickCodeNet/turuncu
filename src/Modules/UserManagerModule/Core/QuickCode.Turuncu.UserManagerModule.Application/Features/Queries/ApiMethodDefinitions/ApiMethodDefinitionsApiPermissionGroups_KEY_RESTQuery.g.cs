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
    public class ApiMethodDefinitionsApiMethodDefinitionsApiPermissionGroups_KEY_RESTQuery : IRequest<Response<ApiMethodDefinitionsApiPermissionGroups_KEY_RESTResponseDto>>
    {
        public int ApiMethodDefinitionsId { get; set; }
        public int ApiPermissionGroupsId { get; set; }

        public ApiMethodDefinitionsApiMethodDefinitionsApiPermissionGroups_KEY_RESTQuery(int apiMethodDefinitionsId, int apiPermissionGroupsId)
        {
            this.ApiMethodDefinitionsId = apiMethodDefinitionsId;
            this.ApiPermissionGroupsId = apiPermissionGroupsId;
        }

        public class ApiMethodDefinitionsApiMethodDefinitionsApiPermissionGroups_KEY_RESTHandler : IRequestHandler<ApiMethodDefinitionsApiMethodDefinitionsApiPermissionGroups_KEY_RESTQuery, Response<ApiMethodDefinitionsApiPermissionGroups_KEY_RESTResponseDto>>
        {
            private readonly ILogger<ApiMethodDefinitionsApiMethodDefinitionsApiPermissionGroups_KEY_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IApiMethodDefinitionsRepository _repository;
            public ApiMethodDefinitionsApiMethodDefinitionsApiPermissionGroups_KEY_RESTHandler(IMapper mapper, ILogger<ApiMethodDefinitionsApiMethodDefinitionsApiPermissionGroups_KEY_RESTHandler> logger, IApiMethodDefinitionsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<ApiMethodDefinitionsApiPermissionGroups_KEY_RESTResponseDto>> Handle(ApiMethodDefinitionsApiMethodDefinitionsApiPermissionGroups_KEY_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<ApiMethodDefinitionsApiPermissionGroups_KEY_RESTResponseDto>>(await _repository.ApiMethodDefinitionsApiPermissionGroups_KEY_RESTAsync(request.ApiMethodDefinitionsId, request.ApiPermissionGroupsId));
                return returnValue;
            }
        }
    }
}