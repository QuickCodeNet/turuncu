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
    public class RefreshTokensUpdateRefreshTokenCommand : IRequest<Response<int>>
    {
        public string RefreshTokensToken { get; set; }
        public RefreshTokensUpdateRefreshTokenRequestDto UpdateRequest { get; set; }

        public RefreshTokensUpdateRefreshTokenCommand(string refreshTokensToken, RefreshTokensUpdateRefreshTokenRequestDto updateRequest)
        {
            this.RefreshTokensToken = refreshTokensToken;
            this.UpdateRequest = updateRequest;
        }

        public class RefreshTokensUpdateRefreshTokenHandler : IRequestHandler<RefreshTokensUpdateRefreshTokenCommand, Response<int>>
        {
            private readonly ILogger<RefreshTokensUpdateRefreshTokenHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IRefreshTokensRepository _repository;
            public RefreshTokensUpdateRefreshTokenHandler(IMapper mapper, ILogger<RefreshTokensUpdateRefreshTokenHandler> logger, IRefreshTokensRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(RefreshTokensUpdateRefreshTokenCommand request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<int>>(await _repository.RefreshTokensUpdateRefreshTokenAsync(request.RefreshTokensToken, request.UpdateRequest));
                return returnValue;
            }
        }
    }
}