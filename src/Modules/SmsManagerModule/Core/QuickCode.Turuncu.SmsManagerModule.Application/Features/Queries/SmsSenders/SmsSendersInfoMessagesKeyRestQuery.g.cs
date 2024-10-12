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
    public class SmsSendersSmsSendersInfoMessagesKeyRestQuery : IRequest<Response<SmsSendersInfoMessagesKeyRestResponseDto>>
    {
        public int SmsSendersId { get; set; }
        public int InfoMessagesId { get; set; }

        public SmsSendersSmsSendersInfoMessagesKeyRestQuery(int smsSendersId, int infoMessagesId)
        {
            this.SmsSendersId = smsSendersId;
            this.InfoMessagesId = infoMessagesId;
        }

        public class SmsSendersSmsSendersInfoMessagesKeyRestHandler : IRequestHandler<SmsSendersSmsSendersInfoMessagesKeyRestQuery, Response<SmsSendersInfoMessagesKeyRestResponseDto>>
        {
            private readonly ILogger<SmsSendersSmsSendersInfoMessagesKeyRestHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ISmsSendersRepository _repository;
            public SmsSendersSmsSendersInfoMessagesKeyRestHandler(IMapper mapper, ILogger<SmsSendersSmsSendersInfoMessagesKeyRestHandler> logger, ISmsSendersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<SmsSendersInfoMessagesKeyRestResponseDto>> Handle(SmsSendersSmsSendersInfoMessagesKeyRestQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<SmsSendersInfoMessagesKeyRestResponseDto>>(await _repository.SmsSendersInfoMessagesKeyRestAsync(request.SmsSendersId, request.InfoMessagesId));
                return returnValue;
            }
        }
    }
}