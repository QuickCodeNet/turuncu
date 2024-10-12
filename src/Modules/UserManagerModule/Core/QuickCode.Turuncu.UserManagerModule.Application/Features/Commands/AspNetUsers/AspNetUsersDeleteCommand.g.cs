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
    public class AspNetUsersDeleteCommand : IRequest<Response<bool>>
    {
        public AspNetUsersDto request { get; set; }

        public AspNetUsersDeleteCommand(AspNetUsersDto request)
        {
            this.request = request;
        }

        public class AspNetUsersDeleteHandler : IRequestHandler<AspNetUsersDeleteCommand, Response<bool>>
        {
            private readonly ILogger<AspNetUsersDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUsersRepository _repository;
            public AspNetUsersDeleteHandler(IMapper mapper, ILogger<AspNetUsersDeleteHandler> logger, IAspNetUsersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(AspNetUsersDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<AspNetUsers>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}