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
    public class AspNetUsersAspNetUsersAspNetUserClaims_RESTQuery : IRequest<Response<List<AspNetUsersAspNetUserClaims_RESTResponseDto>>>
    {
        public string AspNetUsersId { get; set; }

        public AspNetUsersAspNetUsersAspNetUserClaims_RESTQuery(string aspNetUsersId)
        {
            this.AspNetUsersId = aspNetUsersId;
        }

        public class AspNetUsersAspNetUsersAspNetUserClaims_RESTHandler : IRequestHandler<AspNetUsersAspNetUsersAspNetUserClaims_RESTQuery, Response<List<AspNetUsersAspNetUserClaims_RESTResponseDto>>>
        {
            private readonly ILogger<AspNetUsersAspNetUsersAspNetUserClaims_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUsersRepository _repository;
            public AspNetUsersAspNetUsersAspNetUserClaims_RESTHandler(IMapper mapper, ILogger<AspNetUsersAspNetUsersAspNetUserClaims_RESTHandler> logger, IAspNetUsersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<AspNetUsersAspNetUserClaims_RESTResponseDto>>> Handle(AspNetUsersAspNetUsersAspNetUserClaims_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<AspNetUsersAspNetUserClaims_RESTResponseDto>>>(await _repository.AspNetUsersAspNetUserClaims_RESTAsync(request.AspNetUsersId));
                return returnValue;
            }
        }
    }
}