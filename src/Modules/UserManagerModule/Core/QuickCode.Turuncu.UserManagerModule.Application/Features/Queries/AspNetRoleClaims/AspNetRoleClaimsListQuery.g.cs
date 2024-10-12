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
    public class AspNetRoleClaimsListQuery : IRequest<Response<List<AspNetRoleClaimsDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public AspNetRoleClaimsListQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class AspNetRoleClaimsListHandler : IRequestHandler<AspNetRoleClaimsListQuery, Response<List<AspNetRoleClaimsDto>>>
        {
            private readonly ILogger<AspNetRoleClaimsListHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetRoleClaimsRepository _repository;
            public AspNetRoleClaimsListHandler(IMapper mapper, ILogger<AspNetRoleClaimsListHandler> logger, IAspNetRoleClaimsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<AspNetRoleClaimsDto>>> Handle(AspNetRoleClaimsListQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<AspNetRoleClaimsDto>>>(await _repository.ListAsync(request.PageNumber, request.PageSize));
                return returnValue;
            }
        }
    }
}