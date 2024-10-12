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
    public class PermissionGroupsListQuery : IRequest<Response<List<PermissionGroupsDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public PermissionGroupsListQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class PermissionGroupsListHandler : IRequestHandler<PermissionGroupsListQuery, Response<List<PermissionGroupsDto>>>
        {
            private readonly ILogger<PermissionGroupsListHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPermissionGroupsRepository _repository;
            public PermissionGroupsListHandler(IMapper mapper, ILogger<PermissionGroupsListHandler> logger, IPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<PermissionGroupsDto>>> Handle(PermissionGroupsListQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<PermissionGroupsDto>>>(await _repository.ListAsync(request.PageNumber, request.PageSize));
                return returnValue;
            }
        }
    }
}