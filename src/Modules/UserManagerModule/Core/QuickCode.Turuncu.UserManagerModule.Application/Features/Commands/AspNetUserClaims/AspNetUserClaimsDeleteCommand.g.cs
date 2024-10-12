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
    public class AspNetUserClaimsDeleteCommand : IRequest<Response<bool>>
    {
        public AspNetUserClaimsDto request { get; set; }

        public AspNetUserClaimsDeleteCommand(AspNetUserClaimsDto request)
        {
            this.request = request;
        }

        public class AspNetUserClaimsDeleteHandler : IRequestHandler<AspNetUserClaimsDeleteCommand, Response<bool>>
        {
            private readonly ILogger<AspNetUserClaimsDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserClaimsRepository _repository;
            public AspNetUserClaimsDeleteHandler(IMapper mapper, ILogger<AspNetUserClaimsDeleteHandler> logger, IAspNetUserClaimsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(AspNetUserClaimsDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<AspNetUserClaims>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}