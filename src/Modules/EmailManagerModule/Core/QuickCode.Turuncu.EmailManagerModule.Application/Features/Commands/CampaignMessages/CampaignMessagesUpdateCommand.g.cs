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
    public class CampaignMessagesUpdateCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
        public CampaignMessagesDto request { get; set; }

        public CampaignMessagesUpdateCommand(int id, CampaignMessagesDto request)
        {
            this.request = request;
            this.Id = id;
        }

        public class CampaignMessagesUpdateHandler : IRequestHandler<CampaignMessagesUpdateCommand, Response<bool>>
        {
            private readonly ILogger<CampaignMessagesUpdateHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ICampaignMessagesRepository _repository;
            public CampaignMessagesUpdateHandler(IMapper mapper, ILogger<CampaignMessagesUpdateHandler> logger, ICampaignMessagesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(CampaignMessagesUpdateCommand request, CancellationToken cancellationToken)
            {
                var updateItem = await _repository.GetByPkAsync(request.Id);
                if (updateItem.Code == 404)
                {
                    return new Response<bool>()
                    {
                        Code = 404,
                        Value = false
                    };
                }

                var model = _mapper.Map<CampaignMessages>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.UpdateAsync(model));
                return returnValue;
            }
        }
    }
}