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
    public class RefreshTokensRefreshTokensGetRefreshTokenQuery : IRequest<Response<RefreshTokensGetRefreshTokenResponseDto>>
    {
        public string RefreshTokensToken { get; set; }

        public RefreshTokensRefreshTokensGetRefreshTokenQuery(string refreshTokensToken)
        {
            this.RefreshTokensToken = refreshTokensToken;
        }

        public class RefreshTokensRefreshTokensGetRefreshTokenHandler : IRequestHandler<RefreshTokensRefreshTokensGetRefreshTokenQuery, Response<RefreshTokensGetRefreshTokenResponseDto>>
        {
            private readonly ILogger<RefreshTokensRefreshTokensGetRefreshTokenHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IRefreshTokensRepository _repository;
            public RefreshTokensRefreshTokensGetRefreshTokenHandler(IMapper mapper, ILogger<RefreshTokensRefreshTokensGetRefreshTokenHandler> logger, IRefreshTokensRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<RefreshTokensGetRefreshTokenResponseDto>> Handle(RefreshTokensRefreshTokensGetRefreshTokenQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<RefreshTokensGetRefreshTokenResponseDto>>(await _repository.RefreshTokensGetRefreshTokenAsync(request.RefreshTokensToken));
                return returnValue;
            }
        }
    }
}