using AutoMapper;
using System.Linq;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.Turuncu.UserManagerModule.Application.Models;
using QuickCode.Turuncu.UserManagerModule.Domain.Entities;
using QuickCode.Turuncu.UserManagerModule.Application.Interfaces.Repositories;
using QuickCode.Turuncu.UserManagerModule.Application.Dtos;

namespace QuickCode.Turuncu.UserManagerModule.Application.Features
{
    public class PortalMenusTotalItemCountQuery : IRequest<Response<int>>
    {
        public PortalMenusTotalItemCountQuery()
        {
        }

        public class PortalMenusTotalItemCountHandler : IRequestHandler<PortalMenusTotalItemCountQuery, Response<int>>
        {
            private readonly ILogger<PortalMenusTotalItemCountHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalMenusRepository _repository;
            public PortalMenusTotalItemCountHandler(IMapper mapper, ILogger<PortalMenusTotalItemCountHandler> logger, IPortalMenusRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(PortalMenusTotalItemCountQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<int>>(await _repository.CountAsync());
                return returnValue;
            }
        }
    }
}