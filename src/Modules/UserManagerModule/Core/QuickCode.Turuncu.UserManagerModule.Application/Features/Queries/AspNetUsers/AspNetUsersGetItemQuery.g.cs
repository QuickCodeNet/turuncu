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
    public class AspNetUsersGetItemQuery : IRequest<Response<AspNetUsersDto>>
    {
        public string Id { get; set; }

        public AspNetUsersGetItemQuery(string id)
        {
            this.Id = id;
        }

        public class AspNetUsersGetItemHandler : IRequestHandler<AspNetUsersGetItemQuery, Response<AspNetUsersDto>>
        {
            private readonly ILogger<AspNetUsersGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUsersRepository _repository;
            public AspNetUsersGetItemHandler(IMapper mapper, ILogger<AspNetUsersGetItemHandler> logger, IAspNetUsersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUsersDto>> Handle(AspNetUsersGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<AspNetUsersDto>>(await _repository.GetByPkAsync(request.Id));
                return returnValue;
            }
        }
    }
}