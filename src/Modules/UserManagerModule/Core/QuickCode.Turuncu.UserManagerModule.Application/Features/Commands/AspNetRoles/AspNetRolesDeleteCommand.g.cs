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
    public class AspNetRolesDeleteCommand : IRequest<Response<bool>>
    {
        public AspNetRolesDto request { get; set; }

        public AspNetRolesDeleteCommand(AspNetRolesDto request)
        {
            this.request = request;
        }

        public class AspNetRolesDeleteHandler : IRequestHandler<AspNetRolesDeleteCommand, Response<bool>>
        {
            private readonly ILogger<AspNetRolesDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetRolesRepository _repository;
            public AspNetRolesDeleteHandler(IMapper mapper, ILogger<AspNetRolesDeleteHandler> logger, IAspNetRolesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(AspNetRolesDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<AspNetRoles>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}