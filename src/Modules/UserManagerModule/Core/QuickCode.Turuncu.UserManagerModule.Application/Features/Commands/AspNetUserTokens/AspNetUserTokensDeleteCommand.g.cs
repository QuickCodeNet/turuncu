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
    public class AspNetUserTokensDeleteCommand : IRequest<Response<bool>>
    {
        public AspNetUserTokensDto request { get; set; }

        public AspNetUserTokensDeleteCommand(AspNetUserTokensDto request)
        {
            this.request = request;
        }

        public class AspNetUserTokensDeleteHandler : IRequestHandler<AspNetUserTokensDeleteCommand, Response<bool>>
        {
            private readonly ILogger<AspNetUserTokensDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserTokensRepository _repository;
            public AspNetUserTokensDeleteHandler(IMapper mapper, ILogger<AspNetUserTokensDeleteHandler> logger, IAspNetUserTokensRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(AspNetUserTokensDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<AspNetUserTokens>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}