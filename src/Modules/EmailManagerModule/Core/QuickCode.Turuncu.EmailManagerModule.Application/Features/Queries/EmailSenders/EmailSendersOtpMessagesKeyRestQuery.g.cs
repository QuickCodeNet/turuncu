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
    public class EmailSendersEmailSendersOtpMessagesKeyRestQuery : IRequest<Response<EmailSendersOtpMessagesKeyRestResponseDto>>
    {
        public int EmailSendersId { get; set; }
        public int OtpMessagesId { get; set; }

        public EmailSendersEmailSendersOtpMessagesKeyRestQuery(int emailSendersId, int otpMessagesId)
        {
            this.EmailSendersId = emailSendersId;
            this.OtpMessagesId = otpMessagesId;
        }

        public class EmailSendersEmailSendersOtpMessagesKeyRestHandler : IRequestHandler<EmailSendersEmailSendersOtpMessagesKeyRestQuery, Response<EmailSendersOtpMessagesKeyRestResponseDto>>
        {
            private readonly ILogger<EmailSendersEmailSendersOtpMessagesKeyRestHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IEmailSendersRepository _repository;
            public EmailSendersEmailSendersOtpMessagesKeyRestHandler(IMapper mapper, ILogger<EmailSendersEmailSendersOtpMessagesKeyRestHandler> logger, IEmailSendersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<EmailSendersOtpMessagesKeyRestResponseDto>> Handle(EmailSendersEmailSendersOtpMessagesKeyRestQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<EmailSendersOtpMessagesKeyRestResponseDto>>(await _repository.EmailSendersOtpMessagesKeyRestAsync(request.EmailSendersId, request.OtpMessagesId));
                return returnValue;
            }
        }
    }
}