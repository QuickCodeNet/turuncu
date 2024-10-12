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
    public class ApiMethodDefinitionsDeleteCommand : IRequest<Response<bool>>
    {
        public ApiMethodDefinitionsDto request { get; set; }

        public ApiMethodDefinitionsDeleteCommand(ApiMethodDefinitionsDto request)
        {
            this.request = request;
        }

        public class ApiMethodDefinitionsDeleteHandler : IRequestHandler<ApiMethodDefinitionsDeleteCommand, Response<bool>>
        {
            private readonly ILogger<ApiMethodDefinitionsDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IApiMethodDefinitionsRepository _repository;
            public ApiMethodDefinitionsDeleteHandler(IMapper mapper, ILogger<ApiMethodDefinitionsDeleteHandler> logger, IApiMethodDefinitionsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(ApiMethodDefinitionsDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<ApiMethodDefinitions>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}