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
    public class KafkaEventsListQuery : IRequest<Response<List<KafkaEventsDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public KafkaEventsListQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class KafkaEventsListHandler : IRequestHandler<KafkaEventsListQuery, Response<List<KafkaEventsDto>>>
        {
            private readonly ILogger<KafkaEventsListHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IKafkaEventsRepository _repository;
            public KafkaEventsListHandler(IMapper mapper, ILogger<KafkaEventsListHandler> logger, IKafkaEventsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<KafkaEventsDto>>> Handle(KafkaEventsListQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<KafkaEventsDto>>>(await _repository.ListAsync(request.PageNumber, request.PageSize));
                return returnValue;
            }
        }
    }
}