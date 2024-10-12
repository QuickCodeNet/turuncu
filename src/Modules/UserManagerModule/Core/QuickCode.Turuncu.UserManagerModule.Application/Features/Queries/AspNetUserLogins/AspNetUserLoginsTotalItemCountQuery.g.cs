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
    public class AspNetUserLoginsTotalItemCountQuery : IRequest<Response<int>>
    {
        public AspNetUserLoginsTotalItemCountQuery()
        {
        }

        public class AspNetUserLoginsTotalItemCountHandler : IRequestHandler<AspNetUserLoginsTotalItemCountQuery, Response<int>>
        {
            private readonly ILogger<AspNetUserLoginsTotalItemCountHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserLoginsRepository _repository;
            public AspNetUserLoginsTotalItemCountHandler(IMapper mapper, ILogger<AspNetUserLoginsTotalItemCountHandler> logger, IAspNetUserLoginsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(AspNetUserLoginsTotalItemCountQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<int>>(await _repository.CountAsync());
                return returnValue;
            }
        }
    }
}