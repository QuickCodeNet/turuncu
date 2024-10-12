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
    public class AspNetUserClaimsInsertCommand : IRequest<Response<AspNetUserClaimsDto>>
    {
        public AspNetUserClaimsDto request { get; set; }

        public AspNetUserClaimsInsertCommand(AspNetUserClaimsDto request)
        {
            this.request = request;
        }

        public class AspNetUserClaimsInsertHandler : IRequestHandler<AspNetUserClaimsInsertCommand, Response<AspNetUserClaimsDto>>
        {
            private readonly ILogger<AspNetUserClaimsInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserClaimsRepository _repository;
            public AspNetUserClaimsInsertHandler(IMapper mapper, ILogger<AspNetUserClaimsInsertHandler> logger, IAspNetUserClaimsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUserClaimsDto>> Handle(AspNetUserClaimsInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<AspNetUserClaims>(request.request);
                var returnValue = _mapper.Map<Response<AspNetUserClaimsDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}