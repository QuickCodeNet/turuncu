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
    public class RefreshTokensListQuery : IRequest<Response<List<RefreshTokensDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public RefreshTokensListQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class RefreshTokensListHandler : IRequestHandler<RefreshTokensListQuery, Response<List<RefreshTokensDto>>>
        {
            private readonly ILogger<RefreshTokensListHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IRefreshTokensRepository _repository;
            public RefreshTokensListHandler(IMapper mapper, ILogger<RefreshTokensListHandler> logger, IRefreshTokensRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<RefreshTokensDto>>> Handle(RefreshTokensListQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<RefreshTokensDto>>>(await _repository.ListAsync(request.PageNumber, request.PageSize));
                return returnValue;
            }
        }
    }
}