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
    public class ColumnTypesGetItemQuery : IRequest<Response<ColumnTypesDto>>
    {
        public int Id { get; set; }

        public ColumnTypesGetItemQuery(int id)
        {
            this.Id = id;
        }

        public class ColumnTypesGetItemHandler : IRequestHandler<ColumnTypesGetItemQuery, Response<ColumnTypesDto>>
        {
            private readonly ILogger<ColumnTypesGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IColumnTypesRepository _repository;
            public ColumnTypesGetItemHandler(IMapper mapper, ILogger<ColumnTypesGetItemHandler> logger, IColumnTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<ColumnTypesDto>> Handle(ColumnTypesGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<ColumnTypesDto>>(await _repository.GetByPkAsync(request.Id));
                return returnValue;
            }
        }
    }
}