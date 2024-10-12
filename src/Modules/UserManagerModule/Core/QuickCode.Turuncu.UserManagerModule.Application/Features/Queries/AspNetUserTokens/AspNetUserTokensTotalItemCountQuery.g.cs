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
    public class AspNetUserTokensTotalItemCountQuery : IRequest<Response<int>>
    {
        public AspNetUserTokensTotalItemCountQuery()
        {
        }

        public class AspNetUserTokensTotalItemCountHandler : IRequestHandler<AspNetUserTokensTotalItemCountQuery, Response<int>>
        {
            private readonly ILogger<AspNetUserTokensTotalItemCountHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserTokensRepository _repository;
            public AspNetUserTokensTotalItemCountHandler(IMapper mapper, ILogger<AspNetUserTokensTotalItemCountHandler> logger, IAspNetUserTokensRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(AspNetUserTokensTotalItemCountQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<int>>(await _repository.CountAsync());
                return returnValue;
            }
        }
    }
}