using AutoMapper;
using System.Linq;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.Turuncu.SmsManagerModule.Application.Models;
using QuickCode.Turuncu.SmsManagerModule.Domain.Entities;
using QuickCode.Turuncu.SmsManagerModule.Application.Interfaces.Repositories;
using QuickCode.Turuncu.SmsManagerModule.Application.Dtos;

namespace QuickCode.Turuncu.SmsManagerModule.Application.Features
{
    public class SmsSendersSmsSendersInfoMessagesRestQuery : IRequest<Response<List<SmsSendersInfoMessagesRestResponseDto>>>
    {
        public int SmsSendersId { get; set; }

        public SmsSendersSmsSendersInfoMessagesRestQuery(int smsSendersId)
        {
            this.SmsSendersId = smsSendersId;
        }

        public class SmsSendersSmsSendersInfoMessagesRestHandler : IRequestHandler<SmsSendersSmsSendersInfoMessagesRestQuery, Response<List<SmsSendersInfoMessagesRestResponseDto>>>
        {
            private readonly ILogger<SmsSendersSmsSendersInfoMessagesRestHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ISmsSendersRepository _repository;
            public SmsSendersSmsSendersInfoMessagesRestHandler(IMapper mapper, ILogger<SmsSendersSmsSendersInfoMessagesRestHandler> logger, ISmsSendersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<SmsSendersInfoMessagesRestResponseDto>>> Handle(SmsSendersSmsSendersInfoMessagesRestQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<SmsSendersInfoMessagesRestResponseDto>>>(await _repository.SmsSendersInfoMessagesRestAsync(request.SmsSendersId));
                return returnValue;
            }
        }
    }
}