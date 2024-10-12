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
    public class InfoMessagesTotalItemCountQuery : IRequest<Response<int>>
    {
        public InfoMessagesTotalItemCountQuery()
        {
        }

        public class InfoMessagesTotalItemCountHandler : IRequestHandler<InfoMessagesTotalItemCountQuery, Response<int>>
        {
            private readonly ILogger<InfoMessagesTotalItemCountHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IInfoMessagesRepository _repository;
            public InfoMessagesTotalItemCountHandler(IMapper mapper, ILogger<InfoMessagesTotalItemCountHandler> logger, IInfoMessagesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(InfoMessagesTotalItemCountQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<int>>(await _repository.CountAsync());
                return returnValue;
            }
        }
    }
}