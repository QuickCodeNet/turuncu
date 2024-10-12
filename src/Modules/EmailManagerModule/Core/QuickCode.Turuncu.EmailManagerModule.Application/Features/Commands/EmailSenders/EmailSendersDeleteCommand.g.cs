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
    public class EmailSendersDeleteCommand : IRequest<Response<bool>>
    {
        public EmailSendersDto request { get; set; }

        public EmailSendersDeleteCommand(EmailSendersDto request)
        {
            this.request = request;
        }

        public class EmailSendersDeleteHandler : IRequestHandler<EmailSendersDeleteCommand, Response<bool>>
        {
            private readonly ILogger<EmailSendersDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IEmailSendersRepository _repository;
            public EmailSendersDeleteHandler(IMapper mapper, ILogger<EmailSendersDeleteHandler> logger, IEmailSendersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(EmailSendersDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<EmailSenders>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}