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
    public class TopicWorkflowsTotalItemCountQuery : IRequest<Response<int>>
    {
        public TopicWorkflowsTotalItemCountQuery()
        {
        }

        public class TopicWorkflowsTotalItemCountHandler : IRequestHandler<TopicWorkflowsTotalItemCountQuery, Response<int>>
        {
            private readonly ILogger<TopicWorkflowsTotalItemCountHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ITopicWorkflowsRepository _repository;
            public TopicWorkflowsTotalItemCountHandler(IMapper mapper, ILogger<TopicWorkflowsTotalItemCountHandler> logger, ITopicWorkflowsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(TopicWorkflowsTotalItemCountQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<int>>(await _repository.CountAsync());
                return returnValue;
            }
        }
    }
}