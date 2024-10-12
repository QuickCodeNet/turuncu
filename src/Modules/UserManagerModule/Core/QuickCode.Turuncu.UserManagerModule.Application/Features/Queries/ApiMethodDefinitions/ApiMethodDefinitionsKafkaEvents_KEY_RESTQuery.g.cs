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
    public class ApiMethodDefinitionsApiMethodDefinitionsKafkaEvents_KEY_RESTQuery : IRequest<Response<ApiMethodDefinitionsKafkaEvents_KEY_RESTResponseDto>>
    {
        public int ApiMethodDefinitionsId { get; set; }
        public int KafkaEventsId { get; set; }

        public ApiMethodDefinitionsApiMethodDefinitionsKafkaEvents_KEY_RESTQuery(int apiMethodDefinitionsId, int kafkaEventsId)
        {
            this.ApiMethodDefinitionsId = apiMethodDefinitionsId;
            this.KafkaEventsId = kafkaEventsId;
        }

        public class ApiMethodDefinitionsApiMethodDefinitionsKafkaEvents_KEY_RESTHandler : IRequestHandler<ApiMethodDefinitionsApiMethodDefinitionsKafkaEvents_KEY_RESTQuery, Response<ApiMethodDefinitionsKafkaEvents_KEY_RESTResponseDto>>
        {
            private readonly ILogger<ApiMethodDefinitionsApiMethodDefinitionsKafkaEvents_KEY_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IApiMethodDefinitionsRepository _repository;
            public ApiMethodDefinitionsApiMethodDefinitionsKafkaEvents_KEY_RESTHandler(IMapper mapper, ILogger<ApiMethodDefinitionsApiMethodDefinitionsKafkaEvents_KEY_RESTHandler> logger, IApiMethodDefinitionsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<ApiMethodDefinitionsKafkaEvents_KEY_RESTResponseDto>> Handle(ApiMethodDefinitionsApiMethodDefinitionsKafkaEvents_KEY_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<ApiMethodDefinitionsKafkaEvents_KEY_RESTResponseDto>>(await _repository.ApiMethodDefinitionsKafkaEvents_KEY_RESTAsync(request.ApiMethodDefinitionsId, request.KafkaEventsId));
                return returnValue;
            }
        }
    }
}