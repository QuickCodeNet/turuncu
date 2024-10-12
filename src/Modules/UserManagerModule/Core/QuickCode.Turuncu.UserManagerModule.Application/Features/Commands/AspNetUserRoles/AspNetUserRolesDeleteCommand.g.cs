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
    public class AspNetUserRolesDeleteCommand : IRequest<Response<bool>>
    {
        public AspNetUserRolesDto request { get; set; }

        public AspNetUserRolesDeleteCommand(AspNetUserRolesDto request)
        {
            this.request = request;
        }

        public class AspNetUserRolesDeleteHandler : IRequestHandler<AspNetUserRolesDeleteCommand, Response<bool>>
        {
            private readonly ILogger<AspNetUserRolesDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserRolesRepository _repository;
            public AspNetUserRolesDeleteHandler(IMapper mapper, ILogger<AspNetUserRolesDeleteHandler> logger, IAspNetUserRolesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(AspNetUserRolesDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<AspNetUserRoles>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}