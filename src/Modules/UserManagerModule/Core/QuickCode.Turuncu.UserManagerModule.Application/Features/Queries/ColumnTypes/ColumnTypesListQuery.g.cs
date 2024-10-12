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
    public class ColumnTypesListQuery : IRequest<Response<List<ColumnTypesDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public ColumnTypesListQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class ColumnTypesListHandler : IRequestHandler<ColumnTypesListQuery, Response<List<ColumnTypesDto>>>
        {
            private readonly ILogger<ColumnTypesListHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IColumnTypesRepository _repository;
            public ColumnTypesListHandler(IMapper mapper, ILogger<ColumnTypesListHandler> logger, IColumnTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<ColumnTypesDto>>> Handle(ColumnTypesListQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<ColumnTypesDto>>>(await _repository.ListAsync(request.PageNumber, request.PageSize));
                return returnValue;
            }
        }
    }
}