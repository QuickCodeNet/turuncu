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
    public class TableComboboxSettingsTotalItemCountQuery : IRequest<Response<int>>
    {
        public TableComboboxSettingsTotalItemCountQuery()
        {
        }

        public class TableComboboxSettingsTotalItemCountHandler : IRequestHandler<TableComboboxSettingsTotalItemCountQuery, Response<int>>
        {
            private readonly ILogger<TableComboboxSettingsTotalItemCountHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ITableComboboxSettingsRepository _repository;
            public TableComboboxSettingsTotalItemCountHandler(IMapper mapper, ILogger<TableComboboxSettingsTotalItemCountHandler> logger, ITableComboboxSettingsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(TableComboboxSettingsTotalItemCountQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<int>>(await _repository.CountAsync());
                return returnValue;
            }
        }
    }
}