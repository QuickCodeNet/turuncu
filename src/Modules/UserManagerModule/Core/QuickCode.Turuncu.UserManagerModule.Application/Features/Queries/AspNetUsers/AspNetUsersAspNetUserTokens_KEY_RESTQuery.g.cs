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
    public class AspNetUsersAspNetUsersAspNetUserTokens_KEY_RESTQuery : IRequest<Response<AspNetUsersAspNetUserTokens_KEY_RESTResponseDto>>
    {
        public string AspNetUsersId { get; set; }
        public string AspNetUserTokensUserId { get; set; }

        public AspNetUsersAspNetUsersAspNetUserTokens_KEY_RESTQuery(string aspNetUsersId, string aspNetUserTokensUserId)
        {
            this.AspNetUsersId = aspNetUsersId;
            this.AspNetUserTokensUserId = aspNetUserTokensUserId;
        }

        public class AspNetUsersAspNetUsersAspNetUserTokens_KEY_RESTHandler : IRequestHandler<AspNetUsersAspNetUsersAspNetUserTokens_KEY_RESTQuery, Response<AspNetUsersAspNetUserTokens_KEY_RESTResponseDto>>
        {
            private readonly ILogger<AspNetUsersAspNetUsersAspNetUserTokens_KEY_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUsersRepository _repository;
            public AspNetUsersAspNetUsersAspNetUserTokens_KEY_RESTHandler(IMapper mapper, ILogger<AspNetUsersAspNetUsersAspNetUserTokens_KEY_RESTHandler> logger, IAspNetUsersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUsersAspNetUserTokens_KEY_RESTResponseDto>> Handle(AspNetUsersAspNetUsersAspNetUserTokens_KEY_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<AspNetUsersAspNetUserTokens_KEY_RESTResponseDto>>(await _repository.AspNetUsersAspNetUserTokens_KEY_RESTAsync(request.AspNetUsersId, request.AspNetUserTokensUserId));
                return returnValue;
            }
        }
    }
}