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
    public class SmsSendersSmsSendersOtpMessagesKeyRestQuery : IRequest<Response<SmsSendersOtpMessagesKeyRestResponseDto>>
    {
        public int SmsSendersId { get; set; }
        public int OtpMessagesId { get; set; }

        public SmsSendersSmsSendersOtpMessagesKeyRestQuery(int smsSendersId, int otpMessagesId)
        {
            this.SmsSendersId = smsSendersId;
            this.OtpMessagesId = otpMessagesId;
        }

        public class SmsSendersSmsSendersOtpMessagesKeyRestHandler : IRequestHandler<SmsSendersSmsSendersOtpMessagesKeyRestQuery, Response<SmsSendersOtpMessagesKeyRestResponseDto>>
        {
            private readonly ILogger<SmsSendersSmsSendersOtpMessagesKeyRestHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ISmsSendersRepository _repository;
            public SmsSendersSmsSendersOtpMessagesKeyRestHandler(IMapper mapper, ILogger<SmsSendersSmsSendersOtpMessagesKeyRestHandler> logger, ISmsSendersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<SmsSendersOtpMessagesKeyRestResponseDto>> Handle(SmsSendersSmsSendersOtpMessagesKeyRestQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<SmsSendersOtpMessagesKeyRestResponseDto>>(await _repository.SmsSendersOtpMessagesKeyRestAsync(request.SmsSendersId, request.OtpMessagesId));
                return returnValue;
            }
        }
    }
}