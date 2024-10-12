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
    public class AspNetUsersAspNetUsersAspNetUserRoles_KEY_RESTQuery : IRequest<Response<AspNetUsersAspNetUserRoles_KEY_RESTResponseDto>>
    {
        public string AspNetUsersId { get; set; }
        public string AspNetUserRolesUserId { get; set; }

        public AspNetUsersAspNetUsersAspNetUserRoles_KEY_RESTQuery(string aspNetUsersId, string aspNetUserRolesUserId)
        {
            this.AspNetUsersId = aspNetUsersId;
            this.AspNetUserRolesUserId = aspNetUserRolesUserId;
        }

        public class AspNetUsersAspNetUsersAspNetUserRoles_KEY_RESTHandler : IRequestHandler<AspNetUsersAspNetUsersAspNetUserRoles_KEY_RESTQuery, Response<AspNetUsersAspNetUserRoles_KEY_RESTResponseDto>>
        {
            private readonly ILogger<AspNetUsersAspNetUsersAspNetUserRoles_KEY_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUsersRepository _repository;
            public AspNetUsersAspNetUsersAspNetUserRoles_KEY_RESTHandler(IMapper mapper, ILogger<AspNetUsersAspNetUsersAspNetUserRoles_KEY_RESTHandler> logger, IAspNetUsersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUsersAspNetUserRoles_KEY_RESTResponseDto>> Handle(AspNetUsersAspNetUsersAspNetUserRoles_KEY_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<AspNetUsersAspNetUserRoles_KEY_RESTResponseDto>>(await _repository.AspNetUsersAspNetUserRoles_KEY_RESTAsync(request.AspNetUsersId, request.AspNetUserRolesUserId));
                return returnValue;
            }
        }
    }
}