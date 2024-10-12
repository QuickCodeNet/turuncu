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
    public class ApiMethodDefinitionsApiMethodDefinitionsApiPermissionGroups_RESTQuery : IRequest<Response<List<ApiMethodDefinitionsApiPermissionGroups_RESTResponseDto>>>
    {
        public int ApiMethodDefinitionsId { get; set; }

        public ApiMethodDefinitionsApiMethodDefinitionsApiPermissionGroups_RESTQuery(int apiMethodDefinitionsId)
        {
            this.ApiMethodDefinitionsId = apiMethodDefinitionsId;
        }

        public class ApiMethodDefinitionsApiMethodDefinitionsApiPermissionGroups_RESTHandler : IRequestHandler<ApiMethodDefinitionsApiMethodDefinitionsApiPermissionGroups_RESTQuery, Response<List<ApiMethodDefinitionsApiPermissionGroups_RESTResponseDto>>>
        {
            private readonly ILogger<ApiMethodDefinitionsApiMethodDefinitionsApiPermissionGroups_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IApiMethodDefinitionsRepository _repository;
            public ApiMethodDefinitionsApiMethodDefinitionsApiPermissionGroups_RESTHandler(IMapper mapper, ILogger<ApiMethodDefinitionsApiMethodDefinitionsApiPermissionGroups_RESTHandler> logger, IApiMethodDefinitionsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<ApiMethodDefinitionsApiPermissionGroups_RESTResponseDto>>> Handle(ApiMethodDefinitionsApiMethodDefinitionsApiPermissionGroups_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<ApiMethodDefinitionsApiPermissionGroups_RESTResponseDto>>>(await _repository.ApiMethodDefinitionsApiPermissionGroups_RESTAsync(request.ApiMethodDefinitionsId));
                return returnValue;
            }
        }
    }
}