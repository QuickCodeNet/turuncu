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
    public class TopicWorkflowsListQuery : IRequest<Response<List<TopicWorkflowsDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public TopicWorkflowsListQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class TopicWorkflowsListHandler : IRequestHandler<TopicWorkflowsListQuery, Response<List<TopicWorkflowsDto>>>
        {
            private readonly ILogger<TopicWorkflowsListHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ITopicWorkflowsRepository _repository;
            public TopicWorkflowsListHandler(IMapper mapper, ILogger<TopicWorkflowsListHandler> logger, ITopicWorkflowsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<TopicWorkflowsDto>>> Handle(TopicWorkflowsListQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<TopicWorkflowsDto>>>(await _repository.ListAsync(request.PageNumber, request.PageSize));
                return returnValue;
            }
        }
    }
}