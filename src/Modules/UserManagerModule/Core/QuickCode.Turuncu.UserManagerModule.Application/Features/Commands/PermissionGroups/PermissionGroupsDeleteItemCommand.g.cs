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
    public class PermissionGroupsDeleteItemCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }

        public PermissionGroupsDeleteItemCommand(int id)
        {
            this.Id = id;
        }

        public class PermissionGroupsDeleteItemHandler : IRequestHandler<PermissionGroupsDeleteItemCommand, Response<bool>>
        {
            private readonly ILogger<PermissionGroupsDeleteItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPermissionGroupsRepository _repository;
            public PermissionGroupsDeleteItemHandler(IMapper mapper, ILogger<PermissionGroupsDeleteItemHandler> logger, IPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(PermissionGroupsDeleteItemCommand request, CancellationToken cancellationToken)
            {
                var deleteItem = await _repository.GetByPkAsync(request.Id);
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