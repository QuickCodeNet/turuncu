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
    public class AspNetRoleClaimsInsertCommand : IRequest<Response<AspNetRoleClaimsDto>>
    {
        public AspNetRoleClaimsDto request { get; set; }

        public AspNetRoleClaimsInsertCommand(AspNetRoleClaimsDto request)
        {
            this.request = request;
        }

        public class AspNetRoleClaimsInsertHandler : IRequestHandler<AspNetRoleClaimsInsertCommand, Response<AspNetRoleClaimsDto>>
        {
            private readonly ILogger<AspNetRoleClaimsInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetRoleClaimsRepository _repository;
            public AspNetRoleClaimsInsertHandler(IMapper mapper, ILogger<AspNetRoleClaimsInsertHandler> logger, IAspNetRoleClaimsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetRoleClaimsDto>> Handle(AspNetRoleClaimsInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<AspNetRoleClaims>(request.request);
                var returnValue = _mapper.Map<Response<AspNetRoleClaimsDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}