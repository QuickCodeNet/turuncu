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
    public class SmsSendersSmsSendersCampaignMessagesRestQuery : IRequest<Response<List<SmsSendersCampaignMessagesRestResponseDto>>>
    {
        public int SmsSendersId { get; set; }

        public SmsSendersSmsSendersCampaignMessagesRestQuery(int smsSendersId)
        {
            this.SmsSendersId = smsSendersId;
        }

        public class SmsSendersSmsSendersCampaignMessagesRestHandler : IRequestHandler<SmsSendersSmsSendersCampaignMessagesRestQuery, Response<List<SmsSendersCampaignMessagesRestResponseDto>>>
        {
            private readonly ILogger<SmsSendersSmsSendersCampaignMessagesRestHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ISmsSendersRepository _repository;
            public SmsSendersSmsSendersCampaignMessagesRestHandler(IMapper mapper, ILogger<SmsSendersSmsSendersCampaignMessagesRestHandler> logger, ISmsSendersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<SmsSendersCampaignMessagesRestResponseDto>>> Handle(SmsSendersSmsSendersCampaignMessagesRestQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<SmsSendersCampaignMessagesRestResponseDto>>>(await _repository.SmsSendersCampaignMessagesRestAsync(request.SmsSendersId));
                return returnValue;
            }
        }
    }
}