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
    public class OtpMessagesListQuery : IRequest<Response<List<OtpMessagesDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public OtpMessagesListQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class OtpMessagesListHandler : IRequestHandler<OtpMessagesListQuery, Response<List<OtpMessagesDto>>>
        {
            private readonly ILogger<OtpMessagesListHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IOtpMessagesRepository _repository;
            public OtpMessagesListHandler(IMapper mapper, ILogger<OtpMessagesListHandler> logger, IOtpMessagesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<OtpMessagesDto>>> Handle(OtpMessagesListQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<OtpMessagesDto>>>(await _repository.ListAsync(request.PageNumber, request.PageSize));
                return returnValue;
            }
        }
    }
}