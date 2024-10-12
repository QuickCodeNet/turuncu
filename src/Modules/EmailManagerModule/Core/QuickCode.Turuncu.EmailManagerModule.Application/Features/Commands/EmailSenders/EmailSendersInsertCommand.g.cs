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
    public class EmailSendersInsertCommand : IRequest<Response<EmailSendersDto>>
    {
        public EmailSendersDto request { get; set; }

        public EmailSendersInsertCommand(EmailSendersDto request)
        {
            this.request = request;
        }

        public class EmailSendersInsertHandler : IRequestHandler<EmailSendersInsertCommand, Response<EmailSendersDto>>
        {
            private readonly ILogger<EmailSendersInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IEmailSendersRepository _repository;
            public EmailSendersInsertHandler(IMapper mapper, ILogger<EmailSendersInsertHandler> logger, IEmailSendersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<EmailSendersDto>> Handle(EmailSendersInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<EmailSenders>(request.request);
                var returnValue = _mapper.Map<Response<EmailSendersDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}