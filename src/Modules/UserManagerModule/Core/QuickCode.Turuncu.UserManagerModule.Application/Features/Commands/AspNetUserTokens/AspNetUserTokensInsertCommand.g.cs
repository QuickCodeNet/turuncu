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
    public class AspNetUserTokensInsertCommand : IRequest<Response<AspNetUserTokensDto>>
    {
        public AspNetUserTokensDto request { get; set; }

        public AspNetUserTokensInsertCommand(AspNetUserTokensDto request)
        {
            this.request = request;
        }

        public class AspNetUserTokensInsertHandler : IRequestHandler<AspNetUserTokensInsertCommand, Response<AspNetUserTokensDto>>
        {
            private readonly ILogger<AspNetUserTokensInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserTokensRepository _repository;
            public AspNetUserTokensInsertHandler(IMapper mapper, ILogger<AspNetUserTokensInsertHandler> logger, IAspNetUserTokensRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUserTokensDto>> Handle(AspNetUserTokensInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<AspNetUserTokens>(request.request);
                var returnValue = _mapper.Map<Response<AspNetUserTokensDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}