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
    public class CampaignMessagesInsertCommand : IRequest<Response<CampaignMessagesDto>>
    {
        public CampaignMessagesDto request { get; set; }

        public CampaignMessagesInsertCommand(CampaignMessagesDto request)
        {
            this.request = request;
        }

        public class CampaignMessagesInsertHandler : IRequestHandler<CampaignMessagesInsertCommand, Response<CampaignMessagesDto>>
        {
            private readonly ILogger<CampaignMessagesInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ICampaignMessagesRepository _repository;
            public CampaignMessagesInsertHandler(IMapper mapper, ILogger<CampaignMessagesInsertHandler> logger, ICampaignMessagesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<CampaignMessagesDto>> Handle(CampaignMessagesInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<CampaignMessages>(request.request);
                var returnValue = _mapper.Map<Response<CampaignMessagesDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}