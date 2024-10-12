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
    public class InfoTypesInfoTypesInfoMessagesRestQuery : IRequest<Response<List<InfoTypesInfoMessagesRestResponseDto>>>
    {
        public int InfoTypesId { get; set; }

        public InfoTypesInfoTypesInfoMessagesRestQuery(int infoTypesId)
        {
            this.InfoTypesId = infoTypesId;
        }

        public class InfoTypesInfoTypesInfoMessagesRestHandler : IRequestHandler<InfoTypesInfoTypesInfoMessagesRestQuery, Response<List<InfoTypesInfoMessagesRestResponseDto>>>
        {
            private readonly ILogger<InfoTypesInfoTypesInfoMessagesRestHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IInfoTypesRepository _repository;
            public InfoTypesInfoTypesInfoMessagesRestHandler(IMapper mapper, ILogger<InfoTypesInfoTypesInfoMessagesRestHandler> logger, IInfoTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<InfoTypesInfoMessagesRestResponseDto>>> Handle(InfoTypesInfoTypesInfoMessagesRestQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<InfoTypesInfoMessagesRestResponseDto>>>(await _repository.InfoTypesInfoMessagesRestAsync(request.InfoTypesId));
                return returnValue;
            }
        }
    }
}