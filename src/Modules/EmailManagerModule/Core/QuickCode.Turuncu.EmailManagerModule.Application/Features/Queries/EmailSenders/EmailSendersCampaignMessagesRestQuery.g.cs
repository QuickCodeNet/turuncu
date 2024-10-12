using AutoMapper;
using System.Linq;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.Turuncu.EmailManagerModule.Application.Models;
using QuickCode.Turuncu.EmailManagerModule.Domain.Entities;
using QuickCode.Turuncu.EmailManagerModule.Application.Interfaces.Repositories;
using QuickCode.Turuncu.EmailManagerModule.Application.Dtos;

namespace QuickCode.Turuncu.EmailManagerModule.Application.Features
{
    public class EmailSendersEmailSendersCampaignMessagesRestQuery : IRequest<Response<List<EmailSendersCampaignMessagesRestResponseDto>>>
    {
        public int EmailSendersId { get; set; }

        public EmailSendersEmailSendersCampaignMessagesRestQuery(int emailSendersId)
        {
            this.EmailSendersId = emailSendersId;
        }

        public class EmailSendersEmailSendersCampaignMessagesRestHandler : IRequestHandler<EmailSendersEmailSendersCampaignMessagesRestQuery, Response<List<EmailSendersCampaignMessagesRestResponseDto>>>
        {
            private readonly ILogger<EmailSendersEmailSendersCampaignMessagesRestHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IEmailSendersRepository _repository;
            public EmailSendersEmailSendersCampaignMessagesRestHandler(IMapper mapper, ILogger<EmailSendersEmailSendersCampaignMessagesRestHandler> logger, IEmailSendersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<EmailSendersCampaignMessagesRestResponseDto>>> Handle(EmailSendersEmailSendersCampaignMessagesRestQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<EmailSendersCampaignMessagesRestResponseDto>>>(await _repository.EmailSendersCampaignMessagesRestAsync(request.EmailSendersId));
                return returnValue;
            }
        }
    }
}