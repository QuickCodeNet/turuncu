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
    public class PortalMenusGetItemQuery : IRequest<Response<PortalMenusDto>>
    {
        public int Id { get; set; }

        public PortalMenusGetItemQuery(int id)
        {
            this.Id = id;
        }

        public class PortalMenusGetItemHandler : IRequestHandler<PortalMenusGetItemQuery, Response<PortalMenusDto>>
        {
            private readonly ILogger<PortalMenusGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalMenusRepository _repository;
            public PortalMenusGetItemHandler(IMapper mapper, ILogger<PortalMenusGetItemHandler> logger, IPortalMenusRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<PortalMenusDto>> Handle(PortalMenusGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<PortalMenusDto>>(await _repository.GetByPkAsync(request.Id));
                return returnValue;
            }
        }
    }
}