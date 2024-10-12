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
    public class AspNetUsersAspNetUsersAspNetUserTokens_RESTQuery : IRequest<Response<List<AspNetUsersAspNetUserTokens_RESTResponseDto>>>
    {
        public string AspNetUsersId { get; set; }

        public AspNetUsersAspNetUsersAspNetUserTokens_RESTQuery(string aspNetUsersId)
        {
            this.AspNetUsersId = aspNetUsersId;
        }

        public class AspNetUsersAspNetUsersAspNetUserTokens_RESTHandler : IRequestHandler<AspNetUsersAspNetUsersAspNetUserTokens_RESTQuery, Response<List<AspNetUsersAspNetUserTokens_RESTResponseDto>>>
        {
            private readonly ILogger<AspNetUsersAspNetUsersAspNetUserTokens_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUsersRepository _repository;
            public AspNetUsersAspNetUsersAspNetUserTokens_RESTHandler(IMapper mapper, ILogger<AspNetUsersAspNetUsersAspNetUserTokens_RESTHandler> logger, IAspNetUsersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<AspNetUsersAspNetUserTokens_RESTResponseDto>>> Handle(AspNetUsersAspNetUsersAspNetUserTokens_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<AspNetUsersAspNetUserTokens_RESTResponseDto>>>(await _repository.AspNetUsersAspNetUserTokens_RESTAsync(request.AspNetUsersId));
                return returnValue;
            }
        }
    }
}