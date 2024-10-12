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
    public class ColumnTypesInsertCommand : IRequest<Response<ColumnTypesDto>>
    {
        public ColumnTypesDto request { get; set; }

        public ColumnTypesInsertCommand(ColumnTypesDto request)
        {
            this.request = request;
        }

        public class ColumnTypesInsertHandler : IRequestHandler<ColumnTypesInsertCommand, Response<ColumnTypesDto>>
        {
            private readonly ILogger<ColumnTypesInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IColumnTypesRepository _repository;
            public ColumnTypesInsertHandler(IMapper mapper, ILogger<ColumnTypesInsertHandler> logger, IColumnTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<ColumnTypesDto>> Handle(ColumnTypesInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<ColumnTypes>(request.request);
                var returnValue = _mapper.Map<Response<ColumnTypesDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}