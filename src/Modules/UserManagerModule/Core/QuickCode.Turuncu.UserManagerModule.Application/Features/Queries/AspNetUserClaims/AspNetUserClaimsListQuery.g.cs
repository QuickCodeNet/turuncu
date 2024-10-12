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
    public class AspNetUserClaimsListQuery : IRequest<Response<List<AspNetUserClaimsDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public AspNetUserClaimsListQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class AspNetUserClaimsListHandler : IRequestHandler<AspNetUserClaimsListQuery, Response<List<AspNetUserClaimsDto>>>
        {
            private readonly ILogger<AspNetUserClaimsListHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserClaimsRepository _repository;
            public AspNetUserClaimsListHandler(IMapper mapper, ILogger<AspNetUserClaimsListHandler> logger, IAspNetUserClaimsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<AspNetUserClaimsDto>>> Handle(AspNetUserClaimsListQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<AspNetUserClaimsDto>>>(await _repository.ListAsync(request.PageNumber, request.PageSize));
                return returnValue;
            }
        }
    }
}