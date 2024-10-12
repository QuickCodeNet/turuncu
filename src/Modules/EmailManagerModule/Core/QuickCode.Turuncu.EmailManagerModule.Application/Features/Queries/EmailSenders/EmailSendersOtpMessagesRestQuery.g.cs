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
    public class EmailSendersEmailSendersOtpMessagesRestQuery : IRequest<Response<List<EmailSendersOtpMessagesRestResponseDto>>>
    {
        public int EmailSendersId { get; set; }

        public EmailSendersEmailSendersOtpMessagesRestQuery(int emailSendersId)
        {
            this.EmailSendersId = emailSendersId;
        }

        public class EmailSendersEmailSendersOtpMessagesRestHandler : IRequestHandler<EmailSendersEmailSendersOtpMessagesRestQuery, Response<List<EmailSendersOtpMessagesRestResponseDto>>>
        {
            private readonly ILogger<EmailSendersEmailSendersOtpMessagesRestHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IEmailSendersRepository _repository;
            public EmailSendersEmailSendersOtpMessagesRestHandler(IMapper mapper, ILogger<EmailSendersEmailSendersOtpMessagesRestHandler> logger, IEmailSendersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<EmailSendersOtpMessagesRestResponseDto>>> Handle(EmailSendersEmailSendersOtpMessagesRestQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<EmailSendersOtpMessagesRestResponseDto>>>(await _repository.EmailSendersOtpMessagesRestAsync(request.EmailSendersId));
                return returnValue;
            }
        }
    }
}