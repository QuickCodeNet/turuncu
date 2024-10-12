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
    public class EmailSendersGetItemQuery : IRequest<Response<EmailSendersDto>>
    {
        public int Id { get; set; }

        public EmailSendersGetItemQuery(int id)
        {
            this.Id = id;
        }

        public class EmailSendersGetItemHandler : IRequestHandler<EmailSendersGetItemQuery, Response<EmailSendersDto>>
        {
            private readonly ILogger<EmailSendersGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IEmailSendersRepository _repository;
            public EmailSendersGetItemHandler(IMapper mapper, ILogger<EmailSendersGetItemHandler> logger, IEmailSendersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<EmailSendersDto>> Handle(EmailSendersGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<EmailSendersDto>>(await _repository.GetByPkAsync(request.Id));
                return returnValue;
            }
        }
    }
}