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
    public class PortalMenusDeleteCommand : IRequest<Response<bool>>
    {
        public PortalMenusDto request { get; set; }

        public PortalMenusDeleteCommand(PortalMenusDto request)
        {
            this.request = request;
        }

        public class PortalMenusDeleteHandler : IRequestHandler<PortalMenusDeleteCommand, Response<bool>>
        {
            private readonly ILogger<PortalMenusDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalMenusRepository _repository;
            public PortalMenusDeleteHandler(IMapper mapper, ILogger<PortalMenusDeleteHandler> logger, IPortalMenusRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(PortalMenusDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<PortalMenus>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}