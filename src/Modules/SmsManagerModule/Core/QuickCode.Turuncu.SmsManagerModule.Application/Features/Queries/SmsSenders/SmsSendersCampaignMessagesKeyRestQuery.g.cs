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
    public class SmsSendersSmsSendersCampaignMessagesKeyRestQuery : IRequest<Response<SmsSendersCampaignMessagesKeyRestResponseDto>>
    {
        public int SmsSendersId { get; set; }
        public int CampaignMessagesId { get; set; }

        public SmsSendersSmsSendersCampaignMessagesKeyRestQuery(int smsSendersId, int campaignMessagesId)
        {
            this.SmsSendersId = smsSendersId;
            this.CampaignMessagesId = campaignMessagesId;
        }

        public class SmsSendersSmsSendersCampaignMessagesKeyRestHandler : IRequestHandler<SmsSendersSmsSendersCampaignMessagesKeyRestQuery, Response<SmsSendersCampaignMessagesKeyRestResponseDto>>
        {
            private readonly ILogger<SmsSendersSmsSendersCampaignMessagesKeyRestHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ISmsSendersRepository _repository;
            public SmsSendersSmsSendersCampaignMessagesKeyRestHandler(IMapper mapper, ILogger<SmsSendersSmsSendersCampaignMessagesKeyRestHandler> logger, ISmsSendersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<SmsSendersCampaignMessagesKeyRestResponseDto>> Handle(SmsSendersSmsSendersCampaignMessagesKeyRestQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<SmsSendersCampaignMessagesKeyRestResponseDto>>(await _repository.SmsSendersCampaignMessagesKeyRestAsync(request.SmsSendersId, request.CampaignMessagesId));
                return returnValue;
            }
        }
    }
}