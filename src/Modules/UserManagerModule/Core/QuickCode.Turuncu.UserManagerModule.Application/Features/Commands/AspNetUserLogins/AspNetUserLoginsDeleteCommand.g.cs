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
    public class AspNetUserLoginsDeleteCommand : IRequest<Response<bool>>
    {
        public AspNetUserLoginsDto request { get; set; }

        public AspNetUserLoginsDeleteCommand(AspNetUserLoginsDto request)
        {
            this.request = request;
        }

        public class AspNetUserLoginsDeleteHandler : IRequestHandler<AspNetUserLoginsDeleteCommand, Response<bool>>
        {
            private readonly ILogger<AspNetUserLoginsDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IAspNetUserLoginsRepository _repository;
            public AspNetUserLoginsDeleteHandler(IMapper mapper, ILogger<AspNetUserLoginsDeleteHandler> logger, IAspNetUserLoginsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(AspNetUserLoginsDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<AspNetUserLogins>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}