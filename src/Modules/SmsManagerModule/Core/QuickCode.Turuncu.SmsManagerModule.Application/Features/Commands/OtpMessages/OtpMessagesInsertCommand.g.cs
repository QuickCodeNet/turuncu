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
    public class OtpMessagesInsertCommand : IRequest<Response<OtpMessagesDto>>
    {
        public OtpMessagesDto request { get; set; }

        public OtpMessagesInsertCommand(OtpMessagesDto request)
        {
            this.request = request;
        }

        public class OtpMessagesInsertHandler : IRequestHandler<OtpMessagesInsertCommand, Response<OtpMessagesDto>>
        {
            private readonly ILogger<OtpMessagesInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IOtpMessagesRepository _repository;
            public OtpMessagesInsertHandler(IMapper mapper, ILogger<OtpMessagesInsertHandler> logger, IOtpMessagesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<OtpMessagesDto>> Handle(OtpMessagesInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<OtpMessages>(request.request);
                var returnValue = _mapper.Map<Response<OtpMessagesDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}