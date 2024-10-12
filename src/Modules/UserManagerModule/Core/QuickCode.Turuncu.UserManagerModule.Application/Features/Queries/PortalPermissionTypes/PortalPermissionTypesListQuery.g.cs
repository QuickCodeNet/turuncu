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
    public class PortalPermissionTypesListQuery : IRequest<Response<List<PortalPermissionTypesDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public PortalPermissionTypesListQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class PortalPermissionTypesListHandler : IRequestHandler<PortalPermissionTypesListQuery, Response<List<PortalPermissionTypesDto>>>
        {
            private readonly ILogger<PortalPermissionTypesListHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalPermissionTypesRepository _repository;
            public PortalPermissionTypesListHandler(IMapper mapper, ILogger<PortalPermissionTypesListHandler> logger, IPortalPermissionTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<PortalPermissionTypesDto>>> Handle(PortalPermissionTypesListQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<PortalPermissionTypesDto>>>(await _repository.ListAsync(request.PageNumber, request.PageSize));
                return returnValue;
            }
        }
    }
}