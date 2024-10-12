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
    public class AspNetRoleClaimsTotalItemCountQuery : IRequest<Response<int>>
    {
        public AspNetRoleClaimsTotalItemCountQuery()
        {
        }

        public class AspNetRoleClaimsTotalItemCountHandler : IRequestHandler<AspNetRoleClaimsTotalItemCountQuery, Response<int>>
        {
            private readonly ILogger<AspNetRoleClaimsTotalItemCountHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetRoleClaimsRepository _repository;
            public AspNetRoleClaimsTotalItemCountHandler(IMapper mapper, ILogger<AspNetRoleClaimsTotalItemCountHandler> logger, IAspNetRoleClaimsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(AspNetRoleClaimsTotalItemCountQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<int>>(await _repository.CountAsync());
                return returnValue;
            }
        }
    }
}