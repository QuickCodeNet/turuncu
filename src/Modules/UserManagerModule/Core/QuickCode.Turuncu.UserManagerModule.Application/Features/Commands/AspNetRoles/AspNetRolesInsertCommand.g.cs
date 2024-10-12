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
    public class AspNetRolesInsertCommand : IRequest<Response<AspNetRolesDto>>
    {
        public AspNetRolesDto request { get; set; }

        public AspNetRolesInsertCommand(AspNetRolesDto request)
        {
            this.request = request;
        }

        public class AspNetRolesInsertHandler : IRequestHandler<AspNetRolesInsertCommand, Response<AspNetRolesDto>>
        {
            private readonly ILogger<AspNetRolesInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetRolesRepository _repository;
            public AspNetRolesInsertHandler(IMapper mapper, ILogger<AspNetRolesInsertHandler> logger, IAspNetRolesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetRolesDto>> Handle(AspNetRolesInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<AspNetRoles>(request.request);
                var returnValue = _mapper.Map<Response<AspNetRolesDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}