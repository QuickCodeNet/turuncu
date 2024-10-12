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
    public class AspNetUsersInsertCommand : IRequest<Response<AspNetUsersDto>>
    {
        public AspNetUsersDto request { get; set; }

        public AspNetUsersInsertCommand(AspNetUsersDto request)
        {
            this.request = request;
        }

        public class AspNetUsersInsertHandler : IRequestHandler<AspNetUsersInsertCommand, Response<AspNetUsersDto>>
        {
            private readonly ILogger<AspNetUsersInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUsersRepository _repository;
            public AspNetUsersInsertHandler(IMapper mapper, ILogger<AspNetUsersInsertHandler> logger, IAspNetUsersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUsersDto>> Handle(AspNetUsersInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<AspNetUsers>(request.request);
                var returnValue = _mapper.Map<Response<AspNetUsersDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}