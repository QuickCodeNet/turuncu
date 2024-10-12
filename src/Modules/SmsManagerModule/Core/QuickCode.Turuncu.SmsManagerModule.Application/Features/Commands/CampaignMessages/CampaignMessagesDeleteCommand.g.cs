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
    public class CampaignMessagesDeleteCommand : IRequest<Response<bool>>
    {
        public CampaignMessagesDto request { get; set; }

        public CampaignMessagesDeleteCommand(CampaignMessagesDto request)
        {
            this.request = request;
        }

        public class CampaignMessagesDeleteHandler : IRequestHandler<CampaignMessagesDeleteCommand, Response<bool>>
        {
            private readonly ILogger<CampaignMessagesDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ICampaignMessagesRepository _repository;
            public CampaignMessagesDeleteHandler(IMapper mapper, ILogger<CampaignMessagesDeleteHandler> logger, ICampaignMessagesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(CampaignMessagesDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<CampaignMessages>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}