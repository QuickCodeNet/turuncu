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
    public class AspNetUserTokensGetItemQuery : IRequest<Response<AspNetUserTokensDto>>
    {
        public string UserId { get; set; }

        public AspNetUserTokensGetItemQuery(string userId)
        {
            this.UserId = userId;
        }

        public class AspNetUserTokensGetItemHandler : IRequestHandler<AspNetUserTokensGetItemQuery, Response<AspNetUserTokensDto>>
        {
            private readonly ILogger<AspNetUserTokensGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserTokensRepository _repository;
            public AspNetUserTokensGetItemHandler(IMapper mapper, ILogger<AspNetUserTokensGetItemHandler> logger, IAspNetUserTokensRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUserTokensDto>> Handle(AspNetUserTokensGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<AspNetUserTokensDto>>(await _repository.GetByPkAsync(request.UserId));
                return returnValue;
            }
        }
    }
}