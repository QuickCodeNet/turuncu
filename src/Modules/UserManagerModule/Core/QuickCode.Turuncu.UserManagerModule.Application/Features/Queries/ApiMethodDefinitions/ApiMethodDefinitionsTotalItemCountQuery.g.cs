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
    public class ApiMethodDefinitionsTotalItemCountQuery : IRequest<Response<int>>
    {
        public ApiMethodDefinitionsTotalItemCountQuery()
        {
        }

        public class ApiMethodDefinitionsTotalItemCountHandler : IRequestHandler<ApiMethodDefinitionsTotalItemCountQuery, Response<int>>
        {
            private readonly ILogger<ApiMethodDefinitionsTotalItemCountHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IApiMethodDefinitionsRepository _repository;
            public ApiMethodDefinitionsTotalItemCountHandler(IMapper mapper, ILogger<ApiMethodDefinitionsTotalItemCountHandler> logger, IApiMethodDefinitionsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(ApiMethodDefinitionsTotalItemCountQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<int>>(await _repository.CountAsync());
                return returnValue;
            }
        }
    }
}