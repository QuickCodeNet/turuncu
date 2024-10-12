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
    public class TableComboboxSettingsListQuery : IRequest<Response<List<TableComboboxSettingsDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public TableComboboxSettingsListQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class TableComboboxSettingsListHandler : IRequestHandler<TableComboboxSettingsListQuery, Response<List<TableComboboxSettingsDto>>>
        {
            private readonly ILogger<TableComboboxSettingsListHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ITableComboboxSettingsRepository _repository;
            public TableComboboxSettingsListHandler(IMapper mapper, ILogger<TableComboboxSettingsListHandler> logger, ITableComboboxSettingsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<TableComboboxSettingsDto>>> Handle(TableComboboxSettingsListQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<List<TableComboboxSettingsDto>>>(await _repository.ListAsync(request.PageNumber, request.PageSize));
                return returnValue;
            }
        }
    }
}