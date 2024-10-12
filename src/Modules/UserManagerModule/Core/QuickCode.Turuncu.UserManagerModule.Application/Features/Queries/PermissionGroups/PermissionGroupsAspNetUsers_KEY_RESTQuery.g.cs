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
    public class PermissionGroupsPermissionGroupsAspNetUsers_KEY_RESTQuery : IRequest<Response<PermissionGroupsAspNetUsers_KEY_RESTResponseDto>>
    {
        public int PermissionGroupsId { get; set; }
        public string AspNetUsersId { get; set; }

        public PermissionGroupsPermissionGroupsAspNetUsers_KEY_RESTQuery(int permissionGroupsId, string aspNetUsersId)
        {
            this.PermissionGroupsId = permissionGroupsId;
            this.AspNetUsersId = aspNetUsersId;
        }

        public class PermissionGroupsPermissionGroupsAspNetUsers_KEY_RESTHandler : IRequestHandler<PermissionGroupsPermissionGroupsAspNetUsers_KEY_RESTQuery, Response<PermissionGroupsAspNetUsers_KEY_RESTResponseDto>>
        {
            private readonly ILogger<PermissionGroupsPermissionGroupsAspNetUsers_KEY_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPermissionGroupsRepository _repository;
            public PermissionGroupsPermissionGroupsAspNetUsers_KEY_RESTHandler(IMapper mapper, ILogger<PermissionGroupsPermissionGroupsAspNetUsers_KEY_RESTHandler> logger, IPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<PermissionGroupsAspNetUsers_KEY_RESTResponseDto>> Handle(PermissionGroupsPermissionGroupsAspNetUsers_KEY_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<PermissionGroupsAspNetUsers_KEY_RESTResponseDto>>(await _repository.PermissionGroupsAspNetUsers_KEY_RESTAsync(request.PermissionGroupsId, request.AspNetUsersId));
                return returnValue;
            }
        }
    }
}