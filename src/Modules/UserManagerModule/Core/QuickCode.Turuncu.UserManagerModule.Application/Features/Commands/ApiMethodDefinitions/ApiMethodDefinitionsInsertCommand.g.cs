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
    public class ApiMethodDefinitionsInsertCommand : IRequest<Response<ApiMethodDefinitionsDto>>
    {
        public ApiMethodDefinitionsDto request { get; set; }

        public ApiMethodDefinitionsInsertCommand(ApiMethodDefinitionsDto request)
        {
            this.request = request;
        }

        public class ApiMethodDefinitionsInsertHandler : IRequestHandler<ApiMethodDefinitionsInsertCommand, Response<ApiMethodDefinitionsDto>>
        {
            private readonly ILogger<ApiMethodDefinitionsInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IApiMethodDefinitionsRepository _repository;
            public ApiMethodDefinitionsInsertHandler(IMapper mapper, ILogger<ApiMethodDefinitionsInsertHandler> logger, IApiMethodDefinitionsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<ApiMethodDefinitionsDto>> Handle(ApiMethodDefinitionsInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<ApiMethodDefinitions>(request.request);
                var returnValue = _mapper.Map<Response<ApiMethodDefinitionsDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}