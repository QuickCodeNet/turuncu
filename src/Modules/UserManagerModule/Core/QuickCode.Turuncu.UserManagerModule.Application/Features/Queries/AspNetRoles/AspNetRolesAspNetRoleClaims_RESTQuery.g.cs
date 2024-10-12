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
    public class AspNetRolesAspNetRolesAspNetRoleClaims_RESTQuery : IRequest<Response<List<AspNetRolesAspNetRoleClaims_RESTResponseDto>>>
    {
        public string AspNetRolesId { get; set; }

        public AspNetRolesAspNetRolesAspNetRoleClaims_RESTQuery(string aspNetRolesId)
        {
            this.AspNetRolesId = aspNetRolesId;
        }

        public class AspNetRolesAspNetRolesAspNetRoleClaims_RESTHandler : IRequestHandler<AspNetRolesAspNetRolesAspNetRoleClaims_RESTQuery, Response<List<AspNetRolesAspNetRoleClaims_RESTResponseDto>>>
        {
            private readonly ILogger<AspNetRolesAspNetRolesAspNetRoleClaims_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetRolesRepository _repository;
            public AspNetRolesAspNetRolesAspNetRoleClaims_RESTHandler(IMapper mapper, ILogger<AspNetRolesAspNetRolesAspNetRoleClaims_RESTHandler> logger, IAspNetRolesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<AspNetRolesAspNetRoleClaims_RESTResponseDto>>> Handle(AspNetRolesAspNetRolesAspNetRoleClaims_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<AspNetRolesAspNetRoleClaims_RESTResponseDto>>>(await _repository.AspNetRolesAspNetRoleClaims_RESTAsync(request.AspNetRolesId));
                return returnValue;
            }
        }
    }
}