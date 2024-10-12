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
    public class AspNetUserLoginsGetItemQuery : IRequest<Response<AspNetUserLoginsDto>>
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }

        public AspNetUserLoginsGetItemQuery(string loginProvider, string providerKey)
        {
            this.LoginProvider = loginProvider;
            this.ProviderKey = providerKey;
        }

        public class AspNetUserLoginsGetItemHandler : IRequestHandler<AspNetUserLoginsGetItemQuery, Response<AspNetUserLoginsDto>>
        {
            private readonly ILogger<AspNetUserLoginsGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserLoginsRepository _repository;
            public AspNetUserLoginsGetItemHandler(IMapper mapper, ILogger<AspNetUserLoginsGetItemHandler> logger, IAspNetUserLoginsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUserLoginsDto>> Handle(AspNetUserLoginsGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<AspNetUserLoginsDto>>(await _repository.GetByPkAsync(request.LoginProvider, request.ProviderKey));
                return returnValue;
            }
        }
    }
}