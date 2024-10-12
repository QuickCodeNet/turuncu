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
    public class RefreshTokensTotalItemCountQuery : IRequest<Response<int>>
    {
        public RefreshTokensTotalItemCountQuery()
        {
        }

        public class RefreshTokensTotalItemCountHandler : IRequestHandler<RefreshTokensTotalItemCountQuery, Response<int>>
        {
            private readonly ILogger<RefreshTokensTotalItemCountHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IRefreshTokensRepository _repository;
            public RefreshTokensTotalItemCountHandler(IMapper mapper, ILogger<RefreshTokensTotalItemCountHandler> logger, IRefreshTokensRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(RefreshTokensTotalItemCountQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<int>>(await _repository.CountAsync());
                return returnValue;
            }
        }
    }
}