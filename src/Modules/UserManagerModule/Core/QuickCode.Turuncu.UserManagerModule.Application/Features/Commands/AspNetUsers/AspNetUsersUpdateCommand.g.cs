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
    public class AspNetUsersUpdateCommand : IRequest<Response<bool>>
    {
        public string Id { get; set; }
        public AspNetUsersDto request { get; set; }

        public AspNetUsersUpdateCommand(string id, AspNetUsersDto request)
        {
            this.request = request;
            this.Id = id;
        }

        public class AspNetUsersUpdateHandler : IRequestHandler<AspNetUsersUpdateCommand, Response<bool>>
        {
            private readonly ILogger<AspNetUsersUpdateHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUsersRepository _repository;
            public AspNetUsersUpdateHandler(IMapper mapper, ILogger<AspNetUsersUpdateHandler> logger, IAspNetUsersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(AspNetUsersUpdateCommand request, CancellationToken cancellationToken)
            {
                var updateItem = await _repository.GetByPkAsync(request.Id);
                if (updateItem.Code == 404)
                {
                    return new Response<bool>()
                    {
                        Code = 404,
                        Value = false
                    };
                }

                var model = _mapper.Map<AspNetUsers>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.UpdateAsync(model));
                return returnValue;
            }
        }
    }
}