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
    public class TopicWorkflowsInsertCommand : IRequest<Response<TopicWorkflowsDto>>
    {
        public TopicWorkflowsDto request { get; set; }

        public TopicWorkflowsInsertCommand(TopicWorkflowsDto request)
        {
            this.request = request;
        }

        public class TopicWorkflowsInsertHandler : IRequestHandler<TopicWorkflowsInsertCommand, Response<TopicWorkflowsDto>>
        {
            private readonly ILogger<TopicWorkflowsInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ITopicWorkflowsRepository _repository;
            public TopicWorkflowsInsertHandler(IMapper mapper, ILogger<TopicWorkflowsInsertHandler> logger, ITopicWorkflowsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<TopicWorkflowsDto>> Handle(TopicWorkflowsInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<TopicWorkflows>(request.request);
                var returnValue = _mapper.Map<Response<TopicWorkflowsDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}