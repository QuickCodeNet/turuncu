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
    public class SmsSendersListQuery : IRequest<Response<List<SmsSendersDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public SmsSendersListQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class SmsSendersListHandler : IRequestHandler<SmsSendersListQuery, Response<List<SmsSendersDto>>>
        {
            private readonly ILogger<SmsSendersListHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ISmsSendersRepository _repository;
            public SmsSendersListHandler(IMapper mapper, ILogger<SmsSendersListHandler> logger, ISmsSendersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<SmsSendersDto>>> Handle(SmsSendersListQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<SmsSendersDto>>>(await _repository.ListAsync(request.PageNumber, request.PageSize));
                return returnValue;
            }
        }
    }
}