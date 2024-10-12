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
    public class AspNetUserLoginsInsertCommand : IRequest<Response<AspNetUserLoginsDto>>
    {
        public AspNetUserLoginsDto request { get; set; }

        public AspNetUserLoginsInsertCommand(AspNetUserLoginsDto request)
        {
            this.request = request;
        }

        public class AspNetUserLoginsInsertHandler : IRequestHandler<AspNetUserLoginsInsertCommand, Response<AspNetUserLoginsDto>>
        {
            private readonly ILogger<AspNetUserLoginsInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserLoginsRepository _repository;
            public AspNetUserLoginsInsertHandler(IMapper mapper, ILogger<AspNetUserLoginsInsertHandler> logger, IAspNetUserLoginsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUserLoginsDto>> Handle(AspNetUserLoginsInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<AspNetUserLogins>(request.request);
                var returnValue = _mapper.Map<Response<AspNetUserLoginsDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}