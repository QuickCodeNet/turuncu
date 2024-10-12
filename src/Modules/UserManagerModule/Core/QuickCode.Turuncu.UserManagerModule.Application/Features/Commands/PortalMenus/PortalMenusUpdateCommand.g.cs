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
    public class PortalMenusUpdateCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
        public PortalMenusDto request { get; set; }

        public PortalMenusUpdateCommand(int id, PortalMenusDto request)
        {
            this.request = request;
            this.Id = id;
        }

        public class PortalMenusUpdateHandler : IRequestHandler<PortalMenusUpdateCommand, Response<bool>>
        {
            private readonly ILogger<PortalMenusUpdateHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IPortalMenusRepository _repository;
            public PortalMenusUpdateHandler(IMapper mapper, ILogger<PortalMenusUpdateHandler> logger, IPortalMenusRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(PortalMenusUpdateCommand request, CancellationToken cancellationToken)
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

                var model = _mapper.Map<PortalMenus>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.UpdateAsync(model));
                return returnValue;
            }
        }
    }
}