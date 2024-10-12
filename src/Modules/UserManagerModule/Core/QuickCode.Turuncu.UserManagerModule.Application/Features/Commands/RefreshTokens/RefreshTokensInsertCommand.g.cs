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
    public class RefreshTokensInsertCommand : IRequest<Response<RefreshTokensDto>>
    {
        public RefreshTokensDto request { get; set; }

        public RefreshTokensInsertCommand(RefreshTokensDto request)
        {
            this.request = request;
        }

        public class RefreshTokensInsertHandler : IRequestHandler<RefreshTokensInsertCommand, Response<RefreshTokensDto>>
        {
            private readonly ILogger<RefreshTokensInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IRefreshTokensRepository _repository;
            public RefreshTokensInsertHandler(IMapper mapper, ILogger<RefreshTokensInsertHandler> logger, IRefreshTokensRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<RefreshTokensDto>> Handle(RefreshTokensInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<RefreshTokens>(request.request);
                var returnValue = _mapper.Map<Response<RefreshTokensDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}