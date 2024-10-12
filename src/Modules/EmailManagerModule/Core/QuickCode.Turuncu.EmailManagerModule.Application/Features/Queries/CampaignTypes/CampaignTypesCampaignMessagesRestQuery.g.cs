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
    public class CampaignTypesCampaignTypesCampaignMessagesRestQuery : IRequest<Response<List<CampaignTypesCampaignMessagesRestResponseDto>>>
    {
        public int CampaignTypesId { get; set; }

        public CampaignTypesCampaignTypesCampaignMessagesRestQuery(int campaignTypesId)
        {
            this.CampaignTypesId = campaignTypesId;
        }

        public class CampaignTypesCampaignTypesCampaignMessagesRestHandler : IRequestHandler<CampaignTypesCampaignTypesCampaignMessagesRestQuery, Response<List<CampaignTypesCampaignMessagesRestResponseDto>>>
        {
            private readonly ILogger<CampaignTypesCampaignTypesCampaignMessagesRestHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ICampaignTypesRepository _repository;
            public CampaignTypesCampaignTypesCampaignMessagesRestHandler(IMapper mapper, ILogger<CampaignTypesCampaignTypesCampaignMessagesRestHandler> logger, ICampaignTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<CampaignTypesCampaignMessagesRestResponseDto>>> Handle(CampaignTypesCampaignTypesCampaignMessagesRestQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<CampaignTypesCampaignMessagesRestResponseDto>>>(await _repository.CampaignTypesCampaignMessagesRestAsync(request.CampaignTypesId));
                return returnValue;
            }
        }
    }
}