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
    public class SmsSendersGetItemQuery : IRequest<Response<SmsSendersDto>>
    {
        public int Id { get; set; }

        public SmsSendersGetItemQuery(int id)
        {
            this.Id = id;
        }

        public class SmsSendersGetItemHandler : IRequestHandler<SmsSendersGetItemQuery, Response<SmsSendersDto>>
        {
            private readonly ILogger<SmsSendersGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ISmsSendersRepository _repository;
            public SmsSendersGetItemHandler(IMapper mapper, ILogger<SmsSendersGetItemHandler> logger, ISmsSendersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<SmsSendersDto>> Handle(SmsSendersGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<SmsSendersDto>>(await _repository.GetByPkAsync(request.Id));
                return returnValue;
            }
        }
    }
}