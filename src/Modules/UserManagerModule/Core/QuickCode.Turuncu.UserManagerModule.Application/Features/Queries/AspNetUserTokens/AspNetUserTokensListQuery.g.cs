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
    public class AspNetUserTokensListQuery : IRequest<Response<List<AspNetUserTokensDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public AspNetUserTokensListQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class AspNetUserTokensListHandler : IRequestHandler<AspNetUserTokensListQuery, Response<List<AspNetUserTokensDto>>>
        {
            private readonly ILogger<AspNetUserTokensListHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserTokensRepository _repository;
            public AspNetUserTokensListHandler(IMapper mapper, ILogger<AspNetUserTokensListHandler> logger, IAspNetUserTokensRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<AspNetUserTokensDto>>> Handle(AspNetUserTokensListQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<AspNetUserTokensDto>>>(await _repository.ListAsync(request.PageNumber, request.PageSize));
                return returnValue;
            }
        }
    }
}