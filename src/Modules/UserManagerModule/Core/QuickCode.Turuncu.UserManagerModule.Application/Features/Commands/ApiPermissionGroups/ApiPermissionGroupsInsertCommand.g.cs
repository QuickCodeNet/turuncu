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
    public class ApiPermissionGroupsInsertCommand : IRequest<Response<ApiPermissionGroupsDto>>
    {
        public ApiPermissionGroupsDto request { get; set; }

        public ApiPermissionGroupsInsertCommand(ApiPermissionGroupsDto request)
        {
            this.request = request;
        }

        public class ApiPermissionGroupsInsertHandler : IRequestHandler<ApiPermissionGroupsInsertCommand, Response<ApiPermissionGroupsDto>>
        {
            private readonly ILogger<ApiPermissionGroupsInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IApiPermissionGroupsRepository _repository;
            public ApiPermissionGroupsInsertHandler(IMapper mapper, ILogger<ApiPermissionGroupsInsertHandler> logger, IApiPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<ApiPermissionGroupsDto>> Handle(ApiPermissionGroupsInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<ApiPermissionGroups>(request.request);
                var returnValue = _mapper.Map<Response<ApiPermissionGroupsDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}