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
    public class AspNetUserRolesInsertCommand : IRequest<Response<AspNetUserRolesDto>>
    {
        public AspNetUserRolesDto request { get; set; }

        public AspNetUserRolesInsertCommand(AspNetUserRolesDto request)
        {
            this.request = request;
        }

        public class AspNetUserRolesInsertHandler : IRequestHandler<AspNetUserRolesInsertCommand, Response<AspNetUserRolesDto>>
        {
            private readonly ILogger<AspNetUserRolesInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserRolesRepository _repository;
            public AspNetUserRolesInsertHandler(IMapper mapper, ILogger<AspNetUserRolesInsertHandler> logger, IAspNetUserRolesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUserRolesDto>> Handle(AspNetUserRolesInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<AspNetUserRoles>(request.request);
                var returnValue = _mapper.Map<Response<AspNetUserRolesDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}