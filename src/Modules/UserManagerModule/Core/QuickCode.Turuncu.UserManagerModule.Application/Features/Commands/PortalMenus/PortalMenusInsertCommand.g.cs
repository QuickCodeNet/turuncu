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
    public class PortalMenusInsertCommand : IRequest<Response<PortalMenusDto>>
    {
        public PortalMenusDto request { get; set; }

        public PortalMenusInsertCommand(PortalMenusDto request)
        {
            this.request = request;
        }

        public class PortalMenusInsertHandler : IRequestHandler<PortalMenusInsertCommand, Response<PortalMenusDto>>
        {
            private readonly ILogger<PortalMenusInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalMenusRepository _repository;
            public PortalMenusInsertHandler(IMapper mapper, ILogger<PortalMenusInsertHandler> logger, IPortalMenusRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<PortalMenusDto>> Handle(PortalMenusInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<PortalMenus>(request.request);
                var returnValue = _mapper.Map<Response<PortalMenusDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}