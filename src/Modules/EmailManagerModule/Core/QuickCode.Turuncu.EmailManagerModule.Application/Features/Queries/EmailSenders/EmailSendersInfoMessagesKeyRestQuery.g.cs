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
    public class EmailSendersEmailSendersInfoMessagesKeyRestQuery : IRequest<Response<EmailSendersInfoMessagesKeyRestResponseDto>>
    {
        public int EmailSendersId { get; set; }
        public int InfoMessagesId { get; set; }

        public EmailSendersEmailSendersInfoMessagesKeyRestQuery(int emailSendersId, int infoMessagesId)
        {
            this.EmailSendersId = emailSendersId;
            this.InfoMessagesId = infoMessagesId;
        }

        public class EmailSendersEmailSendersInfoMessagesKeyRestHandler : IRequestHandler<EmailSendersEmailSendersInfoMessagesKeyRestQuery, Response<EmailSendersInfoMessagesKeyRestResponseDto>>
        {
            private readonly ILogger<EmailSendersEmailSendersInfoMessagesKeyRestHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IEmailSendersRepository _repository;
            public EmailSendersEmailSendersInfoMessagesKeyRestHandler(IMapper mapper, ILogger<EmailSendersEmailSendersInfoMessagesKeyRestHandler> logger, IEmailSendersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<EmailSendersInfoMessagesKeyRestResponseDto>> Handle(EmailSendersEmailSendersInfoMessagesKeyRestQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<EmailSendersInfoMessagesKeyRestResponseDto>>(await _repository.EmailSendersInfoMessagesKeyRestAsync(request.EmailSendersId, request.InfoMessagesId));
                return returnValue;
            }
        }
    }
}