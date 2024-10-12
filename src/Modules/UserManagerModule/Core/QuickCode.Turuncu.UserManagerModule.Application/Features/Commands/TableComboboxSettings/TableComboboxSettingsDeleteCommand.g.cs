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
    public class TableComboboxSettingsDeleteCommand : IRequest<Response<bool>>
    {
        public TableComboboxSettingsDto request { get; set; }

        public TableComboboxSettingsDeleteCommand(TableComboboxSettingsDto request)
        {
            this.request = request;
        }

        public class TableComboboxSettingsDeleteHandler : IRequestHandler<TableComboboxSettingsDeleteCommand, Response<bool>>
        {
            private readonly ILogger<TableComboboxSettingsDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ITableComboboxSettingsRepository _repository;
            public TableComboboxSettingsDeleteHandler(IMapper mapper, ILogger<TableComboboxSettingsDeleteHandler> logger, ITableComboboxSettingsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(TableComboboxSettingsDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<TableComboboxSettings>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}