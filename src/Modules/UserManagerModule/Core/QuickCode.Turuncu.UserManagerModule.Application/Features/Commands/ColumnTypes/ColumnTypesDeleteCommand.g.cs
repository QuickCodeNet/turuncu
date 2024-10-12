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
    public class ColumnTypesDeleteCommand : IRequest<Response<bool>>
    {
        public ColumnTypesDto request { get; set; }

        public ColumnTypesDeleteCommand(ColumnTypesDto request)
        {
            this.request = request;
        }

        public class ColumnTypesDeleteHandler : IRequestHandler<ColumnTypesDeleteCommand, Response<bool>>
        {
            private readonly ILogger<ColumnTypesDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IColumnTypesRepository _repository;
            public ColumnTypesDeleteHandler(IMapper mapper, ILogger<ColumnTypesDeleteHandler> logger, IColumnTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(ColumnTypesDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<ColumnTypes>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}