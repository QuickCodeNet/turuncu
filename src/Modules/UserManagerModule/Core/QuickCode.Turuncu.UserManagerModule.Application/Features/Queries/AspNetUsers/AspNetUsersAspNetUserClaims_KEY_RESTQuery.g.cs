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
    public class AspNetUsersAspNetUsersAspNetUserClaims_KEY_RESTQuery : IRequest<Response<AspNetUsersAspNetUserClaims_KEY_RESTResponseDto>>
    {
        public string AspNetUsersId { get; set; }
        public int AspNetUserClaimsId { get; set; }

        public AspNetUsersAspNetUsersAspNetUserClaims_KEY_RESTQuery(string aspNetUsersId, int aspNetUserClaimsId)
        {
            this.AspNetUsersId = aspNetUsersId;
            this.AspNetUserClaimsId = aspNetUserClaimsId;
        }

        public class AspNetUsersAspNetUsersAspNetUserClaims_KEY_RESTHandler : IRequestHandler<AspNetUsersAspNetUsersAspNetUserClaims_KEY_RESTQuery, Response<AspNetUsersAspNetUserClaims_KEY_RESTResponseDto>>
        {
            private readonly ILogger<AspNetUsersAspNetUsersAspNetUserClaims_KEY_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUsersRepository _repository;
            public AspNetUsersAspNetUsersAspNetUserClaims_KEY_RESTHandler(IMapper mapper, ILogger<AspNetUsersAspNetUsersAspNetUserClaims_KEY_RESTHandler> logger, IAspNetUsersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUsersAspNetUserClaims_KEY_RESTResponseDto>> Handle(AspNetUsersAspNetUsersAspNetUserClaims_KEY_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<AspNetUsersAspNetUserClaims_KEY_RESTResponseDto>>(await _repository.AspNetUsersAspNetUserClaims_KEY_RESTAsync(request.AspNetUsersId, request.AspNetUserClaimsId));
                return returnValue;
            }
        }
    }
}