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
    public class InfoTypesInfoTypesInfoMessagesKeyRestQuery : IRequest<Response<InfoTypesInfoMessagesKeyRestResponseDto>>
    {
        public int InfoTypesId { get; set; }
        public int InfoMessagesId { get; set; }

        public InfoTypesInfoTypesInfoMessagesKeyRestQuery(int infoTypesId, int infoMessagesId)
        {
            this.InfoTypesId = infoTypesId;
            this.InfoMessagesId = infoMessagesId;
        }

        public class InfoTypesInfoTypesInfoMessagesKeyRestHandler : IRequestHandler<InfoTypesInfoTypesInfoMessagesKeyRestQuery, Response<InfoTypesInfoMessagesKeyRestResponseDto>>
        {
            private readonly ILogger<InfoTypesInfoTypesInfoMessagesKeyRestHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IInfoTypesRepository _repository;
            public InfoTypesInfoTypesInfoMessagesKeyRestHandler(IMapper mapper, ILogger<InfoTypesInfoTypesInfoMessagesKeyRestHandler> logger, IInfoTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<InfoTypesInfoMessagesKeyRestResponseDto>> Handle(InfoTypesInfoTypesInfoMessagesKeyRestQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<InfoTypesInfoMessagesKeyRestResponseDto>>(await _repository.InfoTypesInfoMessagesKeyRestAsync(request.InfoTypesId, request.InfoMessagesId));
                return returnValue;
            }
        }
    }
}