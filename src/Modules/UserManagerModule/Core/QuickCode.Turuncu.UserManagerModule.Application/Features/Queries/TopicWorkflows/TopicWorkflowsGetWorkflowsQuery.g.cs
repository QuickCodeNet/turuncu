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
    public class TopicWorkflowsTopicWorkflowsGetWorkflowsQuery : IRequest<Response<List<TopicWorkflowsGetWorkflowsResponseDto>>>
    {
        public int TopicWorkflowsKafkaEventId { get; set; }

        public TopicWorkflowsTopicWorkflowsGetWorkflowsQuery(int topicWorkflowsKafkaEventId)
        {
            this.TopicWorkflowsKafkaEventId = topicWorkflowsKafkaEventId;
        }

        public class TopicWorkflowsTopicWorkflowsGetWorkflowsHandler : IRequestHandler<TopicWorkflowsTopicWorkflowsGetWorkflowsQuery, Response<List<TopicWorkflowsGetWorkflowsResponseDto>>>
        {
            private readonly ILogger<TopicWorkflowsTopicWorkflowsGetWorkflowsHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ITopicWorkflowsRepository _repository;
            public TopicWorkflowsTopicWorkflowsGetWorkflowsHandler(IMapper mapper, ILogger<TopicWorkflowsTopicWorkflowsGetWorkflowsHandler> logger, ITopicWorkflowsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<TopicWorkflowsGetWorkflowsResponseDto>>> Handle(TopicWorkflowsTopicWorkflowsGetWorkflowsQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<TopicWorkflowsGetWorkflowsResponseDto>>>(await _repository.TopicWorkflowsGetWorkflowsAsync(request.TopicWorkflowsKafkaEventId));
                return returnValue;
            }
        }
    }
}