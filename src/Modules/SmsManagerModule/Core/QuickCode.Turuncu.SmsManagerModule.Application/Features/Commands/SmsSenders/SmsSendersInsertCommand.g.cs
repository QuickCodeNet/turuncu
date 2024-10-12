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
    public class SmsSendersInsertCommand : IRequest<Response<SmsSendersDto>>
    {
        public SmsSendersDto request { get; set; }

        public SmsSendersInsertCommand(SmsSendersDto request)
        {
            this.request = request;
        }

        public class SmsSendersInsertHandler : IRequestHandler<SmsSendersInsertCommand, Response<SmsSendersDto>>
        {
            private readonly ILogger<SmsSendersInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ISmsSendersRepository _repository;
            public SmsSendersInsertHandler(IMapper mapper, ILogger<SmsSendersInsertHandler> logger, ISmsSendersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<SmsSendersDto>> Handle(SmsSendersInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<SmsSenders>(request.request);
                var returnValue = _mapper.Map<Response<SmsSendersDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}