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
    public class OtpTypesOtpTypesOtpMessagesKeyRestQuery : IRequest<Response<OtpTypesOtpMessagesKeyRestResponseDto>>
    {
        public int OtpTypesId { get; set; }
        public int OtpMessagesId { get; set; }

        public OtpTypesOtpTypesOtpMessagesKeyRestQuery(int otpTypesId, int otpMessagesId)
        {
            this.OtpTypesId = otpTypesId;
            this.OtpMessagesId = otpMessagesId;
        }

        public class OtpTypesOtpTypesOtpMessagesKeyRestHandler : IRequestHandler<OtpTypesOtpTypesOtpMessagesKeyRestQuery, Response<OtpTypesOtpMessagesKeyRestResponseDto>>
        {
            private readonly ILogger<OtpTypesOtpTypesOtpMessagesKeyRestHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IOtpTypesRepository _repository;
            public OtpTypesOtpTypesOtpMessagesKeyRestHandler(IMapper mapper, ILogger<OtpTypesOtpTypesOtpMessagesKeyRestHandler> logger, IOtpTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<OtpTypesOtpMessagesKeyRestResponseDto>> Handle(OtpTypesOtpTypesOtpMessagesKeyRestQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<OtpTypesOtpMessagesKeyRestResponseDto>>(await _repository.OtpTypesOtpMessagesKeyRestAsync(request.OtpTypesId, request.OtpMessagesId));
                return returnValue;
            }
        }
    }
}