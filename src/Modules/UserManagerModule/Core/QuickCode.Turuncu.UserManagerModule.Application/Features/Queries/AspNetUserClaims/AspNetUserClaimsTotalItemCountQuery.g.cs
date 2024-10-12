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
    public class AspNetUserClaimsTotalItemCountQuery : IRequest<Response<int>>
    {
        public AspNetUserClaimsTotalItemCountQuery()
        {
        }

        public class AspNetUserClaimsTotalItemCountHandler : IRequestHandler<AspNetUserClaimsTotalItemCountQuery, Response<int>>
        {
            private readonly ILogger<AspNetUserClaimsTotalItemCountHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserClaimsRepository _repository;
            public AspNetUserClaimsTotalItemCountHandler(IMapper mapper, ILogger<AspNetUserClaimsTotalItemCountHandler> logger, IAspNetUserClaimsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(AspNetUserClaimsTotalItemCountQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<int>>(await _repository.CountAsync());
                return returnValue;
            }
        }
    }
}