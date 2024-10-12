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
    public class PermissionGroupsDeleteCommand : IRequest<Response<bool>>
    {
        public PermissionGroupsDto request { get; set; }

        public PermissionGroupsDeleteCommand(PermissionGroupsDto request)
        {
            this.request = request;
        }

        public class PermissionGroupsDeleteHandler : IRequestHandler<PermissionGroupsDeleteCommand, Response<bool>>
        {
            private readonly ILogger<PermissionGroupsDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPermissionGroupsRepository _repository;
            public PermissionGroupsDeleteHandler(IMapper mapper, ILogger<PermissionGroupsDeleteHandler> logger, IPermissionGroupsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(PermissionGroupsDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<PermissionGroups>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}