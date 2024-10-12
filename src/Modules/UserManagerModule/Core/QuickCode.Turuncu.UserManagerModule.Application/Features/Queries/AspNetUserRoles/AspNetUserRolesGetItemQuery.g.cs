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
    public class AspNetUserRolesGetItemQuery : IRequest<Response<AspNetUserRolesDto>>
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public AspNetUserRolesGetItemQuery(string userId, string roleId)
        {
            this.UserId = userId;
            this.RoleId = roleId;
        }

        public class AspNetUserRolesGetItemHandler : IRequestHandler<AspNetUserRolesGetItemQuery, Response<AspNetUserRolesDto>>
        {
            private readonly ILogger<AspNetUserRolesGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserRolesRepository _repository;
            public AspNetUserRolesGetItemHandler(IMapper mapper, ILogger<AspNetUserRolesGetItemHandler> logger, IAspNetUserRolesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUserRolesDto>> Handle(AspNetUserRolesGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<AspNetUserRolesDto>>(await _repository.GetByPkAsync(request.UserId, request.RoleId));
                return returnValue;
            }
        }
    }
}