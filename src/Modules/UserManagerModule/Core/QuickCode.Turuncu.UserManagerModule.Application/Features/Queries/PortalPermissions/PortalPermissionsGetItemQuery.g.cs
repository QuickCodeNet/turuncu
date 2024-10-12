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
    public class PortalPermissionsGetItemQuery : IRequest<Response<PortalPermissionsDto>>
    {
        public int Id { get; set; }

        public PortalPermissionsGetItemQuery(int id)
        {
            this.Id = id;
        }

        public class PortalPermissionsGetItemHandler : IRequestHandler<PortalPermissionsGetItemQuery, Response<PortalPermissionsDto>>
        {
            private readonly ILogger<PortalPermissionsGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalPermissionsRepository _repository;
            public PortalPermissionsGetItemHandler(IMapper mapper, ILogger<PortalPermissionsGetItemHandler> logger, IPortalPermissionsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<PortalPermissionsDto>> Handle(PortalPermissionsGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<PortalPermissionsDto>>(await _repository.GetByPkAsync(request.Id));
                return returnValue;
            }
        }
    }
}