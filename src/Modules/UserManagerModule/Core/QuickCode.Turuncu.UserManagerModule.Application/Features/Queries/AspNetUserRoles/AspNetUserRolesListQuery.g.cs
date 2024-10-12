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
    public class AspNetUserRolesListQuery : IRequest<Response<List<AspNetUserRolesDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public AspNetUserRolesListQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class AspNetUserRolesListHandler : IRequestHandler<AspNetUserRolesListQuery, Response<List<AspNetUserRolesDto>>>
        {
            private readonly ILogger<AspNetUserRolesListHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserRolesRepository _repository;
            public AspNetUserRolesListHandler(IMapper mapper, ILogger<AspNetUserRolesListHandler> logger, IAspNetUserRolesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<AspNetUserRolesDto>>> Handle(AspNetUserRolesListQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<AspNetUserRolesDto>>>(await _repository.ListAsync(request.PageNumber, request.PageSize));
                return returnValue;
            }
        }
    }
}