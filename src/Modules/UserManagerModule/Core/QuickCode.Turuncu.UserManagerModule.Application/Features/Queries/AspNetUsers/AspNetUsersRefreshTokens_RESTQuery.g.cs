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
    public class AspNetUsersAspNetUsersRefreshTokens_RESTQuery : IRequest<Response<List<AspNetUsersRefreshTokens_RESTResponseDto>>>
    {
        public string AspNetUsersId { get; set; }

        public AspNetUsersAspNetUsersRefreshTokens_RESTQuery(string aspNetUsersId)
        {
            this.AspNetUsersId = aspNetUsersId;
        }

        public class AspNetUsersAspNetUsersRefreshTokens_RESTHandler : IRequestHandler<AspNetUsersAspNetUsersRefreshTokens_RESTQuery, Response<List<AspNetUsersRefreshTokens_RESTResponseDto>>>
        {
            private readonly ILogger<AspNetUsersAspNetUsersRefreshTokens_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUsersRepository _repository;
            public AspNetUsersAspNetUsersRefreshTokens_RESTHandler(IMapper mapper, ILogger<AspNetUsersAspNetUsersRefreshTokens_RESTHandler> logger, IAspNetUsersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<AspNetUsersRefreshTokens_RESTResponseDto>>> Handle(AspNetUsersAspNetUsersRefreshTokens_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<AspNetUsersRefreshTokens_RESTResponseDto>>>(await _repository.AspNetUsersRefreshTokens_RESTAsync(request.AspNetUsersId));
                return returnValue;
            }
        }
    }
}