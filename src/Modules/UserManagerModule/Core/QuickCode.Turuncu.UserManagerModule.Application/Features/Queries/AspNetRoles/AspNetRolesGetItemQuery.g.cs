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
    public class AspNetRolesGetItemQuery : IRequest<Response<AspNetRolesDto>>
    {
        public string Id { get; set; }

        public AspNetRolesGetItemQuery(string id)
        {
            this.Id = id;
        }

        public class AspNetRolesGetItemHandler : IRequestHandler<AspNetRolesGetItemQuery, Response<AspNetRolesDto>>
        {
            private readonly ILogger<AspNetRolesGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetRolesRepository _repository;
            public AspNetRolesGetItemHandler(IMapper mapper, ILogger<AspNetRolesGetItemHandler> logger, IAspNetRolesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetRolesDto>> Handle(AspNetRolesGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<AspNetRolesDto>>(await _repository.GetByPkAsync(request.Id));
                return returnValue;
            }
        }
    }
}