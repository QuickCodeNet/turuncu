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
    public class OtpTypesGetItemQuery : IRequest<Response<OtpTypesDto>>
    {
        public int Id { get; set; }

        public OtpTypesGetItemQuery(int id)
        {
            this.Id = id;
        }

        public class OtpTypesGetItemHandler : IRequestHandler<OtpTypesGetItemQuery, Response<OtpTypesDto>>
        {
            private readonly ILogger<OtpTypesGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IOtpTypesRepository _repository;
            public OtpTypesGetItemHandler(IMapper mapper, ILogger<OtpTypesGetItemHandler> logger, IOtpTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<OtpTypesDto>> Handle(OtpTypesGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<OtpTypesDto>>(await _repository.GetByPkAsync(request.Id));
                return returnValue;
            }
        }
    }
}