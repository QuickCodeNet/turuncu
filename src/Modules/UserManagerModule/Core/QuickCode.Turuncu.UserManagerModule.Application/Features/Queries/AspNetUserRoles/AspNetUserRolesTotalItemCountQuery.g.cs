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
    public class AspNetUserRolesTotalItemCountQuery : IRequest<Response<int>>
    {
        public AspNetUserRolesTotalItemCountQuery()
        {
        }

        public class AspNetUserRolesTotalItemCountHandler : IRequestHandler<AspNetUserRolesTotalItemCountQuery, Response<int>>
        {
            private readonly ILogger<AspNetUserRolesTotalItemCountHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserRolesRepository _repository;
            public AspNetUserRolesTotalItemCountHandler(IMapper mapper, ILogger<AspNetUserRolesTotalItemCountHandler> logger, IAspNetUserRolesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(AspNetUserRolesTotalItemCountQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<int>>(await _repository.CountAsync());
                return returnValue;
            }
        }
    }
}