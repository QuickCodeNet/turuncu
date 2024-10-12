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
    public class PortalPermissionsUpdateCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
        public PortalPermissionsDto request { get; set; }

        public PortalPermissionsUpdateCommand(int id, PortalPermissionsDto request)
        {
            this.request = request;
            this.Id = id;
        }

        public class PortalPermissionsUpdateHandler : IRequestHandler<PortalPermissionsUpdateCommand, Response<bool>>
        {
            private readonly ILogger<PortalPermissionsUpdateHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalPermissionsRepository _repository;
            public PortalPermissionsUpdateHandler(IMapper mapper, ILogger<PortalPermissionsUpdateHandler> logger, IPortalPermissionsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(PortalPermissionsUpdateCommand request, CancellationToken cancellationToken)
            {
                var updateItem = await _repository.GetByPkAsync(request.Id);
                if (updateItem.Code == 404)
                {
                    return new Response<bool>()
                    {
                        Code = 404,
                        Value = false
                    };
                }

                var model = _mapper.Map<PortalPermissions>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.UpdateAsync(model));
                return returnValue;
            }
        }
    }
}