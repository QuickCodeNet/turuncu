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
    public class TableComboboxSettingsUpdateCommand : IRequest<Response<bool>>
    {
        public string TableName { get; set; }
        public TableComboboxSettingsDto request { get; set; }

        public TableComboboxSettingsUpdateCommand(string tableName, TableComboboxSettingsDto request)
        {
            this.request = request;
            this.TableName = tableName;
        }

        public class TableComboboxSettingsUpdateHandler : IRequestHandler<TableComboboxSettingsUpdateCommand, Response<bool>>
        {
            private readonly ILogger<TableComboboxSettingsUpdateHandler> _logger;
            private readonly IMapper _mapper;
            private readonly ITableComboboxSettingsRepository _repository;
            public TableComboboxSettingsUpdateHandler(IMapper mapper, ILogger<TableComboboxSettingsUpdateHandler> logger, ITableComboboxSettingsRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(TableComboboxSettingsUpdateCommand request, CancellationToken cancellationToken)
            {
                var updateItem = await _repository.GetByPkAsync(request.TableName);
                if (updateItem.Code == 404)
                {
                    return new Response<bool>()
                    {
                        Code = 404,
                        Value = false
                    };
                }

                var model = _mapper.Map<TableComboboxSettings>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.UpdateAsync(model));
                return returnValue;
            }
        }
    }
}