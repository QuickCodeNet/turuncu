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
    public class OtpTypesOtpTypesOtpMessagesRestQuery : IRequest<Response<List<OtpTypesOtpMessagesRestResponseDto>>>
    {
        public int OtpTypesId { get; set; }

        public OtpTypesOtpTypesOtpMessagesRestQuery(int otpTypesId)
        {
            this.OtpTypesId = otpTypesId;
        }

        public class OtpTypesOtpTypesOtpMessagesRestHandler : IRequestHandler<OtpTypesOtpTypesOtpMessagesRestQuery, Response<List<OtpTypesOtpMessagesRestResponseDto>>>
        {
            private readonly ILogger<OtpTypesOtpTypesOtpMessagesRestHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IOtpTypesRepository _repository;
            public OtpTypesOtpTypesOtpMessagesRestHandler(IMapper mapper, ILogger<OtpTypesOtpTypesOtpMessagesRestHandler> logger, IOtpTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<OtpTypesOtpMessagesRestResponseDto>>> Handle(OtpTypesOtpTypesOtpMessagesRestQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<OtpTypesOtpMessagesRestResponseDto>>>(await _repository.OtpTypesOtpMessagesRestAsync(request.OtpTypesId));
                return returnValue;
            }
        }
    }
}