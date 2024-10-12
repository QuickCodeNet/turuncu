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
    public class PermissionGroupsPermissionGroupsAspNetUsers_RESTQuery : IRequest<Response<List<PermissionGroupsAspNetUsers_RESTResponseDto>>>
    {
        public int PermissionGroupsId { get; set; }

        public PermissionGroupsPermissionGroupsAspNetUsers_RESTQuery(int permissionGroupsId)
        {
            this.PermissionGroupsId = permissionGroupsId;
        }

        public class PermissionGroupsPermissionGroupsAspNetUsers_RESTHandler : IRequestHandler<PermissionGroupsPermissionGroupsAspNetUsers_RESTQuery, Response<List<PermissionGroupsAspNetUsers_RESTResponseDto>>>
        {
            private readonly ILogger<PermissionGroupsPermissionGroupsAspNetUsers_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPermissionGroupsRepository _repository;
            public PermissionGroupsPermissionGroupsAspNetUsers_RESTHandler(IMapper mapper, ILogger<PermissionGroupsPermissionGroupsAspNetUsers_RESTHandler> logger, IPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<PermissionGroupsAspNetUsers_RESTResponseDto>>> Handle(PermissionGroupsPermissionGroupsAspNetUsers_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<PermissionGroupsAspNetUsers_RESTResponseDto>>>(await _repository.PermissionGroupsAspNetUsers_RESTAsync(request.PermissionGroupsId));
                return returnValue;
            }
        }
    }
}