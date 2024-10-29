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
    public class KafkaEventsKafkaEventsTopicWorkflows_RESTQuery : IRequest<Response<List<KafkaEventsTopicWorkflows_RESTResponseDto>>>
    {
        public int KafkaEventsId { get; set; }

        public KafkaEventsKafkaEventsTopicWorkflows_RESTQuery(int kafkaEventsId)
        {
            this.KafkaEventsId = kafkaEventsId;
        }

        public class KafkaEventsKafkaEventsTopicWorkflows_RESTHandler : IRequestHandler<KafkaEventsKafkaEventsTopicWorkflows_RESTQuery, Response<List<KafkaEventsTopicWorkflows_RESTResponseDto>>>
        {
            private readonly ILogger<KafkaEventsKafkaEventsTopicWorkflows_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IKafkaEventsRepository _repository;
            public KafkaEventsKafkaEventsTopicWorkflows_RESTHandler(IMapper mapper, ILogger<KafkaEventsKafkaEventsTopicWorkflows_RESTHandler> logger, IKafkaEventsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<KafkaEventsTopicWorkflows_RESTResponseDto>>> Handle(KafkaEventsKafkaEventsTopicWorkflows_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<KafkaEventsTopicWorkflows_RESTResponseDto>>>(await _repository.KafkaEventsTopicWorkflows_RESTAsync(request.KafkaEventsId));
                return returnValue;
            }
        }
    }
}