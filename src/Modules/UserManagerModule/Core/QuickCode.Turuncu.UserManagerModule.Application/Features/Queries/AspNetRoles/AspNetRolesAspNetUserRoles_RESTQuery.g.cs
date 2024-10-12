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
    public class AspNetRolesAspNetRolesAspNetUserRoles_RESTQuery : IRequest<Response<List<AspNetRolesAspNetUserRoles_RESTResponseDto>>>
    {
        public string AspNetRolesId { get; set; }

        public AspNetRolesAspNetRolesAspNetUserRoles_RESTQuery(string aspNetRolesId)
        {
            this.AspNetRolesId = aspNetRolesId;
        }

        public class AspNetRolesAspNetRolesAspNetUserRoles_RESTHandler : IRequestHandler<AspNetRolesAspNetRolesAspNetUserRoles_RESTQuery, Response<List<AspNetRolesAspNetUserRoles_RESTResponseDto>>>
        {
            private readonly ILogger<AspNetRolesAspNetRolesAspNetUserRoles_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetRolesRepository _repository;
            public AspNetRolesAspNetRolesAspNetUserRoles_RESTHandler(IMapper mapper, ILogger<AspNetRolesAspNetRolesAspNetUserRoles_RESTHandler> logger, IAspNetRolesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<AspNetRolesAspNetUserRoles_RESTResponseDto>>> Handle(AspNetRolesAspNetRolesAspNetUserRoles_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<AspNetRolesAspNetUserRoles_RESTResponseDto>>>(await _repository.AspNetRolesAspNetUserRoles_RESTAsync(request.AspNetRolesId));
                return returnValue;
            }
        }
    }
}