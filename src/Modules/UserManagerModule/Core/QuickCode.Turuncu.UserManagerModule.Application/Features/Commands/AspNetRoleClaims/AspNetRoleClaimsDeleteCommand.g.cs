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
    public class AspNetRoleClaimsDeleteCommand : IRequest<Response<bool>>
    {
        public AspNetRoleClaimsDto request { get; set; }

        public AspNetRoleClaimsDeleteCommand(AspNetRoleClaimsDto request)
        {
            this.request = request;
        }

        public class AspNetRoleClaimsDeleteHandler : IRequestHandler<AspNetRoleClaimsDeleteCommand, Response<bool>>
        {
            private readonly ILogger<AspNetRoleClaimsDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetRoleClaimsRepository _repository;
            public AspNetRoleClaimsDeleteHandler(IMapper mapper, ILogger<AspNetRoleClaimsDeleteHandler> logger, IAspNetRoleClaimsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(AspNetRoleClaimsDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<AspNetRoleClaims>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}