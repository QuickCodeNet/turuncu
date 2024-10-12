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
    public class AspNetUsersAspNetUsersAspNetUserLogins_KEY_RESTQuery : IRequest<Response<AspNetUsersAspNetUserLogins_KEY_RESTResponseDto>>
    {
        public string AspNetUsersId { get; set; }
        public string AspNetUserLoginsLoginProvider { get; set; }

        public AspNetUsersAspNetUsersAspNetUserLogins_KEY_RESTQuery(string aspNetUsersId, string aspNetUserLoginsLoginProvider)
        {
            this.AspNetUsersId = aspNetUsersId;
            this.AspNetUserLoginsLoginProvider = aspNetUserLoginsLoginProvider;
        }

        public class AspNetUsersAspNetUsersAspNetUserLogins_KEY_RESTHandler : IRequestHandler<AspNetUsersAspNetUsersAspNetUserLogins_KEY_RESTQuery, Response<AspNetUsersAspNetUserLogins_KEY_RESTResponseDto>>
        {
            private readonly ILogger<AspNetUsersAspNetUsersAspNetUserLogins_KEY_RESTHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUsersRepository _repository;
            public AspNetUsersAspNetUsersAspNetUserLogins_KEY_RESTHandler(IMapper mapper, ILogger<AspNetUsersAspNetUsersAspNetUserLogins_KEY_RESTHandler> logger, IAspNetUsersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUsersAspNetUserLogins_KEY_RESTResponseDto>> Handle(AspNetUsersAspNetUsersAspNetUserLogins_KEY_RESTQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<AspNetUsersAspNetUserLogins_KEY_RESTResponseDto>>(await _repository.AspNetUsersAspNetUserLogins_KEY_RESTAsync(request.AspNetUsersId, request.AspNetUserLoginsLoginProvider));
                return returnValue;
            }
        }
    }
}