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
    public class AspNetUserRolesDeleteItemCommand : IRequest<Response<bool>>
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public AspNetUserRolesDeleteItemCommand(string userId, string roleId)
        {
            this.UserId = userId;
            this.RoleId = roleId;
        }

        public class AspNetUserRolesDeleteItemHandler : IRequestHandler<AspNetUserRolesDeleteItemCommand, Response<bool>>
        {
            private readonly ILogger<AspNetUserRolesDeleteItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserRolesRepository _repository;
            public AspNetUserRolesDeleteItemHandler(IMapper mapper, ILogger<AspNetUserRolesDeleteItemHandler> logger, IAspNetUserRolesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(AspNetUserRolesDeleteItemCommand request, CancellationToken cancellationToken)
            {
                var deleteItem = await _repository.GetByPkAsync(request.UserId, request.RoleId);
                if (deleteItem.Code == 404)
                {
                    return new Response<bool>()
                    {
                        Code = 404,
                        Value = false
                    };
                }

                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(deleteItem.Value));
                return returnValue;
            }
        }
    }
}