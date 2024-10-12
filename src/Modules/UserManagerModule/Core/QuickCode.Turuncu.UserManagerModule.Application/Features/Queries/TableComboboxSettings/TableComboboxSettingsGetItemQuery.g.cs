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
    public class TableComboboxSettingsGetItemQuery : IRequest<Response<TableComboboxSettingsDto>>
    {
        public string TableName { get; set; }

        public TableComboboxSettingsGetItemQuery(string tableName)
        {
            this.TableName = tableName;
        }

        public class TableComboboxSettingsGetItemHandler : IRequestHandler<TableComboboxSettingsGetItemQuery, Response<TableComboboxSettingsDto>>
        {
            private readonly ILogger<TableComboboxSettingsGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ITableComboboxSettingsRepository _repository;
            public TableComboboxSettingsGetItemHandler(IMapper mapper, ILogger<TableComboboxSettingsGetItemHandler> logger, ITableComboboxSettingsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<TableComboboxSettingsDto>> Handle(TableComboboxSettingsGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<TableComboboxSettingsDto>>(await _repository.GetByPkAsync(request.TableName));
                return returnValue;
            }
        }
    }
}