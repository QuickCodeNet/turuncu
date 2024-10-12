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
    public class AspNetRoleClaimsUpdateCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
        public AspNetRoleClaimsDto request { get; set; }

        public AspNetRoleClaimsUpdateCommand(int id, AspNetRoleClaimsDto request)
        {
            this.request = request;
            this.Id = id;
        }

        public class AspNetRoleClaimsUpdateHandler : IRequestHandler<AspNetRoleClaimsUpdateCommand, Response<bool>>
        {
            private readonly ILogger<AspNetRoleClaimsUpdateHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetRoleClaimsRepository _repository;
            public AspNetRoleClaimsUpdateHandler(IMapper mapper, ILogger<AspNetRoleClaimsUpdateHandler> logger, IAspNetRoleClaimsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(AspNetRoleClaimsUpdateCommand request, CancellationToken cancellationToken)
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

                var model = _mapper.Map<AspNetRoleClaims>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.UpdateAsync(model));
                return returnValue;
            }
        }
    }
}