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
    public class RefreshTokensGetItemQuery : IRequest<Response<RefreshTokensDto>>
    {
        public int Id { get; set; }

        public RefreshTokensGetItemQuery(int id)
        {
            this.Id = id;
        }

        public class RefreshTokensGetItemHandler : IRequestHandler<RefreshTokensGetItemQuery, Response<RefreshTokensDto>>
        {
            private readonly ILogger<RefreshTokensGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IRefreshTokensRepository _repository;
            public RefreshTokensGetItemHandler(IMapper mapper, ILogger<RefreshTokensGetItemHandler> logger, IRefreshTokensRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<RefreshTokensDto>> Handle(RefreshTokensGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<RefreshTokensDto>>(await _repository.GetByPkAsync(request.Id));
                return returnValue;
            }
        }
    }
}