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
    public class ApiMethodDefinitionsGetItemQuery : IRequest<Response<ApiMethodDefinitionsDto>>
    {
        public int Id { get; set; }

        public ApiMethodDefinitionsGetItemQuery(int id)
        {
            this.Id = id;
        }

        public class ApiMethodDefinitionsGetItemHandler : IRequestHandler<ApiMethodDefinitionsGetItemQuery, Response<ApiMethodDefinitionsDto>>
        {
            private readonly ILogger<ApiMethodDefinitionsGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IApiMethodDefinitionsRepository _repository;
            public ApiMethodDefinitionsGetItemHandler(IMapper mapper, ILogger<ApiMethodDefinitionsGetItemHandler> logger, IApiMethodDefinitionsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<ApiMethodDefinitionsDto>> Handle(ApiMethodDefinitionsGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<ApiMethodDefinitionsDto>>(await _repository.GetByPkAsync(request.Id));
                return returnValue;
            }
        }
    }
}