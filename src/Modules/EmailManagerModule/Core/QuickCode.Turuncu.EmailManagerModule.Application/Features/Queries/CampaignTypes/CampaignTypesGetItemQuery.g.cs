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
    public class CampaignTypesGetItemQuery : IRequest<Response<CampaignTypesDto>>
    {
        public int Id { get; set; }

        public CampaignTypesGetItemQuery(int id)
        {
            this.Id = id;
        }

        public class CampaignTypesGetItemHandler : IRequestHandler<CampaignTypesGetItemQuery, Response<CampaignTypesDto>>
        {
            private readonly ILogger<CampaignTypesGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ICampaignTypesRepository _repository;
            public CampaignTypesGetItemHandler(IMapper mapper, ILogger<CampaignTypesGetItemHandler> logger, ICampaignTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<CampaignTypesDto>> Handle(CampaignTypesGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<CampaignTypesDto>>(await _repository.GetByPkAsync(request.Id));
                return returnValue;
            }
        }
    }
}