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
    public class AspNetUserRolesUpdateCommand : IRequest<Response<bool>>
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public AspNetUserRolesDto request { get; set; }

        public AspNetUserRolesUpdateCommand(string userId, string roleId, AspNetUserRolesDto request)
        {
            this.request = request;
            this.UserId = userId;
            this.RoleId = roleId;
        }

        public class AspNetUserRolesUpdateHandler : IRequestHandler<AspNetUserRolesUpdateCommand, Response<bool>>
        {
            private readonly ILogger<AspNetUserRolesUpdateHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserRolesRepository _repository;
            public AspNetUserRolesUpdateHandler(IMapper mapper, ILogger<AspNetUserRolesUpdateHandler> logger, IAspNetUserRolesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(AspNetUserRolesUpdateCommand request, CancellationToken cancellationToken)
            {
                var updateItem = await _repository.GetByPkAsync(request.UserId, request.RoleId);
                if (updateItem.Code == 404)
                {
                    return new Response<bool>()
                    {
                        Code = 404,
                        Value = false
                    };
                }

                var model = _mapper.Map<AspNetUserRoles>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.UpdateAsync(model));
                return returnValue;
            }
        }
    }
}