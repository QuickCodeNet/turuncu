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
    public class AspNetUsersAspNetUsersRefreshTokens_KEY_RESTQuery : IRequest<Response<AspNetUsersRefreshTokens_KEY_RESTResponseDto>>
    {
        public string AspNetUsersId { get; set; }
        public int RefreshTokensId { get; set; }

        public AspNetUsersAspNetUsersRefreshTokens_KEY_RESTQuery(string aspNetUsersId, int refreshTokensId)
        {
            this.AspNetUsersId = aspNetUsersId;
            this.RefreshTokensId = refreshTokensId;
        }

        public class AspNetUsersAspNetUsersRefreshTokens_KEY_RESTHandler : IRequestHandler<AspNetUsersAspNetUsersRefreshTokens_KEY_RESTQuery, Response<AspNetUsersRefreshTokens_KEY_RESTResponseDto>>
        {
            private readonly ILogger<AspNetUsersAspNetUsersRefreshTokens_KEY_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUsersRepository _repository;
            public AspNetUsersAspNetUsersRefreshTokens_KEY_RESTHandler(IMapper mapper, ILogger<AspNetUsersAspNetUsersRefreshTokens_KEY_RESTHandler> logger, IAspNetUsersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUsersRefreshTokens_KEY_RESTResponseDto>> Handle(AspNetUsersAspNetUsersRefreshTokens_KEY_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<AspNetUsersRefreshTokens_KEY_RESTResponseDto>>(await _repository.AspNetUsersRefreshTokens_KEY_RESTAsync(request.AspNetUsersId, request.RefreshTokensId));
                return returnValue;
            }
        }
    }
}