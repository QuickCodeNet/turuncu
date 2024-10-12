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
    public class ColumnTypesDeleteItemCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }

        public ColumnTypesDeleteItemCommand(int id)
        {
            this.Id = id;
        }

        public class ColumnTypesDeleteItemHandler : IRequestHandler<ColumnTypesDeleteItemCommand, Response<bool>>
        {
            private readonly ILogger<ColumnTypesDeleteItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IColumnTypesRepository _repository;
            public ColumnTypesDeleteItemHandler(IMapper mapper, ILogger<ColumnTypesDeleteItemHandler> logger, IColumnTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(ColumnTypesDeleteItemCommand request, CancellationToken cancellationToken)
            {
                var deleteItem = await _repository.GetByPkAsync(request.Id);
                if (deleteItem.Code == 404)
                {
                    return new Response<bool>()
                    {
                        Code = 404,
                        Value = false
                    };
                }

                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(deleteItem.Value));
                return returnValue;
            }
        }
    }
}