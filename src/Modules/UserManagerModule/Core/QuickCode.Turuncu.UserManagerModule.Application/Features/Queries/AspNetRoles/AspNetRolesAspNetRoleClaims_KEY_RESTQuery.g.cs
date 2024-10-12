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
    public class AspNetRolesAspNetRolesAspNetRoleClaims_KEY_RESTQuery : IRequest<Response<AspNetRolesAspNetRoleClaims_KEY_RESTResponseDto>>
    {
        public string AspNetRolesId { get; set; }
        public int AspNetRoleClaimsId { get; set; }

        public AspNetRolesAspNetRolesAspNetRoleClaims_KEY_RESTQuery(string aspNetRolesId, int aspNetRoleClaimsId)
        {
            this.AspNetRolesId = aspNetRolesId;
            this.AspNetRoleClaimsId = aspNetRoleClaimsId;
        }

        public class AspNetRolesAspNetRolesAspNetRoleClaims_KEY_RESTHandler : IRequestHandler<AspNetRolesAspNetRolesAspNetRoleClaims_KEY_RESTQuery, Response<AspNetRolesAspNetRoleClaims_KEY_RESTResponseDto>>
        {
            private readonly ILogger<AspNetRolesAspNetRolesAspNetRoleClaims_KEY_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetRolesRepository _repository;
            public AspNetRolesAspNetRolesAspNetRoleClaims_KEY_RESTHandler(IMapper mapper, ILogger<AspNetRolesAspNetRolesAspNetRoleClaims_KEY_RESTHandler> logger, IAspNetRolesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetRolesAspNetRoleClaims_KEY_RESTResponseDto>> Handle(AspNetRolesAspNetRolesAspNetRoleClaims_KEY_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<AspNetRolesAspNetRoleClaims_KEY_RESTResponseDto>>(await _repository.AspNetRolesAspNetRoleClaims_KEY_RESTAsync(request.AspNetRolesId, request.AspNetRoleClaimsId));
                return returnValue;
            }
        }
    }
}