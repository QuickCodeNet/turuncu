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
    public class OtpMessagesDeleteCommand : IRequest<Response<bool>>
    {
        public OtpMessagesDto request { get; set; }

        public OtpMessagesDeleteCommand(OtpMessagesDto request)
        {
            this.request = request;
        }

        public class OtpMessagesDeleteHandler : IRequestHandler<OtpMessagesDeleteCommand, Response<bool>>
        {
            private readonly ILogger<OtpMessagesDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IOtpMessagesRepository _repository;
            public OtpMessagesDeleteHandler(IMapper mapper, ILogger<OtpMessagesDeleteHandler> logger, IOtpMessagesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(OtpMessagesDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<OtpMessages>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}