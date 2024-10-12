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
    public class TableComboboxSettingsInsertCommand : IRequest<Response<TableComboboxSettingsDto>>
    {
        public TableComboboxSettingsDto request { get; set; }

        public TableComboboxSettingsInsertCommand(TableComboboxSettingsDto request)
        {
            this.request = request;
        }

        public class TableComboboxSettingsInsertHandler : IRequestHandler<TableComboboxSettingsInsertCommand, Response<TableComboboxSettingsDto>>
        {
            private readonly ILogger<TableComboboxSettingsInsertHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ITableComboboxSettingsRepository _repository;
            public TableComboboxSettingsInsertHandler(IMapper mapper, ILogger<TableComboboxSettingsInsertHandler> logger, ITableComboboxSettingsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<TableComboboxSettingsDto>> Handle(TableComboboxSettingsInsertCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<TableComboboxSettings>(request.request);
                var returnValue = _mapper.Map<Response<TableComboboxSettingsDto>>(await _repository.InsertAsync(model));
                return returnValue;
            }
        }
    }
}