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
    public class EmailSendersListQuery : IRequest<Response<List<EmailSendersDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public EmailSendersListQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class EmailSendersListHandler : IRequestHandler<EmailSendersListQuery, Response<List<EmailSendersDto>>>
        {
            private readonly ILogger<EmailSendersListHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IEmailSendersRepository _repository;
            public EmailSendersListHandler(IMapper mapper, ILogger<EmailSendersListHandler> logger, IEmailSendersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<EmailSendersDto>>> Handle(EmailSendersListQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<EmailSendersDto>>>(await _repository.ListAsync(request.PageNumber, request.PageSize));
                return returnValue;
            }
        }
    }
}