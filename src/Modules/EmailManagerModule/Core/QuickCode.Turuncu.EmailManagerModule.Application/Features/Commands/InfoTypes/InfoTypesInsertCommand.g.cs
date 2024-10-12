using AutoMapper;
using System.Linq;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.Turuncu.EmailManagerModule.Application.Models;
using QuickCode.Turuncu.EmailManagerModule.Domain.Entities;
using QuickCode.Turuncu.EmailManagerModule.Application.Interfaces.Repositories;
using QuickCode.Turuncu.EmailManagerModule.Application.Dtos;

namespace QuickCode.Turuncu.EmailManagerModule.Application.Features
{
    public class InfoTypesInsertCommand : IRequest<Response<InfoTypesDto>>
    {
        public InfoTypesDto request { get; set; }

        public InfoTypesInsertCommand(InfoTypesDto request)
        {
            this.request = request;
        }

        public class InfoTypesInsertHandler : IRequestHandler<InfoTypesInsertCommand, Response<InfoTypesDto>>
        {
            private readonly ILogger<InfoTypesInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IInfoTypesRepository _repository;
            public InfoTypesInsertHandler(IMapper mapper, ILogger<InfoTypesInsertHandler> logger, IInfoTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<InfoTypesDto>> Handle(InfoTypesInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<InfoTypes>(request.request);
                var returnValue = _mapper.Map<Response<InfoTypesDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}