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
    public class AspNetRolesAspNetRolesAspNetUserRoles_KEY_RESTQuery : IRequest<Response<AspNetRolesAspNetUserRoles_KEY_RESTResponseDto>>
    {
        public string AspNetRolesId { get; set; }
        public string AspNetUserRolesUserId { get; set; }

        public AspNetRolesAspNetRolesAspNetUserRoles_KEY_RESTQuery(string aspNetRolesId, string aspNetUserRolesUserId)
        {
            this.AspNetRolesId = aspNetRolesId;
            this.AspNetUserRolesUserId = aspNetUserRolesUserId;
        }

        public class AspNetRolesAspNetRolesAspNetUserRoles_KEY_RESTHandler : IRequestHandler<AspNetRolesAspNetRolesAspNetUserRoles_KEY_RESTQuery, Response<AspNetRolesAspNetUserRoles_KEY_RESTResponseDto>>
        {
            private readonly ILogger<AspNetRolesAspNetRolesAspNetUserRoles_KEY_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetRolesRepository _repository;
            public AspNetRolesAspNetRolesAspNetUserRoles_KEY_RESTHandler(IMapper mapper, ILogger<AspNetRolesAspNetRolesAspNetUserRoles_KEY_RESTHandler> logger, IAspNetRolesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetRolesAspNetUserRoles_KEY_RESTResponseDto>> Handle(AspNetRolesAspNetRolesAspNetUserRoles_KEY_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<AspNetRolesAspNetUserRoles_KEY_RESTResponseDto>>(await _repository.AspNetRolesAspNetUserRoles_KEY_RESTAsync(request.AspNetRolesId, request.AspNetUserRolesUserId));
                return returnValue;
            }
        }
    }
}