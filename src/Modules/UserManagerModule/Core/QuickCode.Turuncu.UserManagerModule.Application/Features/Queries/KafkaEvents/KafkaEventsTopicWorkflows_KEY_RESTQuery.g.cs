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
    public class KafkaEventsKafkaEventsTopicWorkflows_KEY_RESTQuery : IRequest<Response<KafkaEventsTopicWorkflows_KEY_RESTResponseDto>>
    {
        public int KafkaEventsId { get; set; }
        public int TopicWorkflowsId { get; set; }

        public KafkaEventsKafkaEventsTopicWorkflows_KEY_RESTQuery(int kafkaEventsId, int topicWorkflowsId)
        {
            this.KafkaEventsId = kafkaEventsId;
            this.TopicWorkflowsId = topicWorkflowsId;
        }

        public class KafkaEventsKafkaEventsTopicWorkflows_KEY_RESTHandler : IRequestHandler<KafkaEventsKafkaEventsTopicWorkflows_KEY_RESTQuery, Response<KafkaEventsTopicWorkflows_KEY_RESTResponseDto>>
        {
            private readonly ILogger<KafkaEventsKafkaEventsTopicWorkflows_KEY_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IKafkaEventsRepository _repository;
            public KafkaEventsKafkaEventsTopicWorkflows_KEY_RESTHandler(IMapper mapper, ILogger<KafkaEventsKafkaEventsTopicWorkflows_KEY_RESTHandler> logger, IKafkaEventsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<KafkaEventsTopicWorkflows_KEY_RESTResponseDto>> Handle(KafkaEventsKafkaEventsTopicWorkflows_KEY_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<KafkaEventsTopicWorkflows_KEY_RESTResponseDto>>(await _repository.KafkaEventsTopicWorkflows_KEY_RESTAsync(request.KafkaEventsId, request.TopicWorkflowsId));
                return returnValue;
            }
        }
    }
}