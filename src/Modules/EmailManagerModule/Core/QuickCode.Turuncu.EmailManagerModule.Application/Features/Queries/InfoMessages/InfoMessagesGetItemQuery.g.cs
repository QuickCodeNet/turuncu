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
    public class InfoMessagesGetItemQuery : IRequest<Response<InfoMessagesDto>>
    {
        public int Id { get; set; }

        public InfoMessagesGetItemQuery(int id)
        {
            this.Id = id;
        }

        public class InfoMessagesGetItemHandler : IRequestHandler<InfoMessagesGetItemQuery, Response<InfoMessagesDto>>
        {
            private readonly ILogger<InfoMessagesGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IInfoMessagesRepository _repository;
            public InfoMessagesGetItemHandler(IMapper mapper, ILogger<InfoMessagesGetItemHandler> logger, IInfoMessagesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<InfoMessagesDto>> Handle(InfoMessagesGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<InfoMessagesDto>>(await _repository.GetByPkAsync(request.Id));
                return returnValue;
            }
        }
    }
}