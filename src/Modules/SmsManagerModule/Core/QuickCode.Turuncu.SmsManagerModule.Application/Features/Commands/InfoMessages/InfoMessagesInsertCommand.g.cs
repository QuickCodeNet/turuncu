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
    public class InfoMessagesInsertCommand : IRequest<Response<InfoMessagesDto>>
    {
        public InfoMessagesDto request { get; set; }

        public InfoMessagesInsertCommand(InfoMessagesDto request)
        {
            this.request = request;
        }

        public class InfoMessagesInsertHandler : IRequestHandler<InfoMessagesInsertCommand, Response<InfoMessagesDto>>
        {
            private readonly ILogger<InfoMessagesInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IInfoMessagesRepository _repository;
            public InfoMessagesInsertHandler(IMapper mapper, ILogger<InfoMessagesInsertHandler> logger, IInfoMessagesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<InfoMessagesDto>> Handle(InfoMessagesInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<InfoMessages>(request.request);
                var returnValue = _mapper.Map<Response<InfoMessagesDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}