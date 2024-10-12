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
    public class ApiPermissionGroupsUpdateCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
        public ApiPermissionGroupsDto request { get; set; }

        public ApiPermissionGroupsUpdateCommand(int id, ApiPermissionGroupsDto request)
        {
            this.request = request;
            this.Id = id;
        }

        public class ApiPermissionGroupsUpdateHandler : IRequestHandler<ApiPermissionGroupsUpdateCommand, Response<bool>>
        {
            private readonly ILogger<ApiPermissionGroupsUpdateHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IApiPermissionGroupsRepository _repository;
            public ApiPermissionGroupsUpdateHandler(IMapper mapper, ILogger<ApiPermissionGroupsUpdateHandler> logger, IApiPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(ApiPermissionGroupsUpdateCommand request, CancellationToken cancellationToken)
            {
                var updateItem = await _repository.GetByPkAsync(request.Id);
                if (updateItem.Code == 404)
                {
                    return new Response<bool>()
                    {
                        Code = 404,
                        Value = false
                    };
                }

                var model = _mapper.Map<ApiPermissionGroups>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.UpdateAsync(model));
                return returnValue;
            }
        }
    }
}