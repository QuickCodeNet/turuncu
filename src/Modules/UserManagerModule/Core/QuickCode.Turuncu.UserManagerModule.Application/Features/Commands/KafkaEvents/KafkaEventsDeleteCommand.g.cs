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
    public class KafkaEventsDeleteCommand : IRequest<Response<bool>>
    {
        public KafkaEventsDto request { get; set; }

        public KafkaEventsDeleteCommand(KafkaEventsDto request)
        {
            this.request = request;
        }

        public class KafkaEventsDeleteHandler : IRequestHandler<KafkaEventsDeleteCommand, Response<bool>>
        {
            private readonly ILogger<KafkaEventsDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IKafkaEventsRepository _repository;
            public KafkaEventsDeleteHandler(IMapper mapper, ILogger<KafkaEventsDeleteHandler> logger, IKafkaEventsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(KafkaEventsDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<KafkaEvents>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}