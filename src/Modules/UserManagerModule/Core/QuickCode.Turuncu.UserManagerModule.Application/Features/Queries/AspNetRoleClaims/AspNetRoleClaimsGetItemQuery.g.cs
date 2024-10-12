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
    public class AspNetRoleClaimsGetItemQuery : IRequest<Response<AspNetRoleClaimsDto>>
    {
        public int Id { get; set; }

        public AspNetRoleClaimsGetItemQuery(int id)
        {
            this.Id = id;
        }

        public class AspNetRoleClaimsGetItemHandler : IRequestHandler<AspNetRoleClaimsGetItemQuery, Response<AspNetRoleClaimsDto>>
        {
            private readonly ILogger<AspNetRoleClaimsGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetRoleClaimsRepository _repository;
            public AspNetRoleClaimsGetItemHandler(IMapper mapper, ILogger<AspNetRoleClaimsGetItemHandler> logger, IAspNetRoleClaimsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetRoleClaimsDto>> Handle(AspNetRoleClaimsGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<AspNetRoleClaimsDto>>(await _repository.GetByPkAsync(request.Id));
                return returnValue;
            }
        }
    }
}