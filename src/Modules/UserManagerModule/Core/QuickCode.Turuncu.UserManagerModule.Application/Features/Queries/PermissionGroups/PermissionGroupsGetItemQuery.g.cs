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
    public class PermissionGroupsGetItemQuery : IRequest<Response<PermissionGroupsDto>>
    {
        public int Id { get; set; }

        public PermissionGroupsGetItemQuery(int id)
        {
            this.Id = id;
        }

        public class PermissionGroupsGetItemHandler : IRequestHandler<PermissionGroupsGetItemQuery, Response<PermissionGroupsDto>>
        {
            private readonly ILogger<PermissionGroupsGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPermissionGroupsRepository _repository;
            public PermissionGroupsGetItemHandler(IMapper mapper, ILogger<PermissionGroupsGetItemHandler> logger, IPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<PermissionGroupsDto>> Handle(PermissionGroupsGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<PermissionGroupsDto>>(await _repository.GetByPkAsync(request.Id));
                return returnValue;
            }
        }
    }
}