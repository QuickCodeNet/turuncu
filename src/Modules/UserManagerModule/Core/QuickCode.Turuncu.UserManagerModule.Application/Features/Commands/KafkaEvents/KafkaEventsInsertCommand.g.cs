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
    public class KafkaEventsInsertCommand : IRequest<Response<KafkaEventsDto>>
    {
        public KafkaEventsDto request { get; set; }

        public KafkaEventsInsertCommand(KafkaEventsDto request)
        {
            this.request = request;
        }

        public class KafkaEventsInsertHandler : IRequestHandler<KafkaEventsInsertCommand, Response<KafkaEventsDto>>
        {
            private readonly ILogger<KafkaEventsInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IKafkaEventsRepository _repository;
            public KafkaEventsInsertHandler(IMapper mapper, ILogger<KafkaEventsInsertHandler> logger, IKafkaEventsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<KafkaEventsDto>> Handle(KafkaEventsInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<KafkaEvents>(request.request);
                var returnValue = _mapper.Map<Response<KafkaEventsDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}