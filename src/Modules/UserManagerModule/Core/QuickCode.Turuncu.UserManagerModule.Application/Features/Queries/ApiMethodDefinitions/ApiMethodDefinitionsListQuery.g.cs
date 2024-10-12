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
    public class ApiMethodDefinitionsListQuery : IRequest<Response<List<ApiMethodDefinitionsDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public ApiMethodDefinitionsListQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class ApiMethodDefinitionsListHandler : IRequestHandler<ApiMethodDefinitionsListQuery, Response<List<ApiMethodDefinitionsDto>>>
        {
            private readonly ILogger<ApiMethodDefinitionsListHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IApiMethodDefinitionsRepository _repository;
            public ApiMethodDefinitionsListHandler(IMapper mapper, ILogger<ApiMethodDefinitionsListHandler> logger, IApiMethodDefinitionsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<ApiMethodDefinitionsDto>>> Handle(ApiMethodDefinitionsListQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<ApiMethodDefinitionsDto>>>(await _repository.ListAsync(request.PageNumber, request.PageSize));
                return returnValue;
            }
        }
    }
}