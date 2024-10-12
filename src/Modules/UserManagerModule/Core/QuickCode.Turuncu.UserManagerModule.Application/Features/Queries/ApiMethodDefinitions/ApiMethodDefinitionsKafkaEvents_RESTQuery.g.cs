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
    public class ApiMethodDefinitionsApiMethodDefinitionsKafkaEvents_RESTQuery : IRequest<Response<List<ApiMethodDefinitionsKafkaEvents_RESTResponseDto>>>
    {
        public int ApiMethodDefinitionsId { get; set; }

        public ApiMethodDefinitionsApiMethodDefinitionsKafkaEvents_RESTQuery(int apiMethodDefinitionsId)
        {
            this.ApiMethodDefinitionsId = apiMethodDefinitionsId;
        }

        public class ApiMethodDefinitionsApiMethodDefinitionsKafkaEvents_RESTHandler : IRequestHandler<ApiMethodDefinitionsApiMethodDefinitionsKafkaEvents_RESTQuery, Response<List<ApiMethodDefinitionsKafkaEvents_RESTResponseDto>>>
        {
            private readonly ILogger<ApiMethodDefinitionsApiMethodDefinitionsKafkaEvents_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IApiMethodDefinitionsRepository _repository;
            public ApiMethodDefinitionsApiMethodDefinitionsKafkaEvents_RESTHandler(IMapper mapper, ILogger<ApiMethodDefinitionsApiMethodDefinitionsKafkaEvents_RESTHandler> logger, IApiMethodDefinitionsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<ApiMethodDefinitionsKafkaEvents_RESTResponseDto>>> Handle(ApiMethodDefinitionsApiMethodDefinitionsKafkaEvents_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<ApiMethodDefinitionsKafkaEvents_RESTResponseDto>>>(await _repository.ApiMethodDefinitionsKafkaEvents_RESTAsync(request.ApiMethodDefinitionsId));
                return returnValue;
            }
        }
    }
}