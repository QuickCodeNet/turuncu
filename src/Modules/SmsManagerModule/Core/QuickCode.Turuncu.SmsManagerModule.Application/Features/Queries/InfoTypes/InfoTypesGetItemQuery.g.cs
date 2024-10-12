using AutoMapper;
using System.Linq;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.Turuncu.SmsManagerModule.Application.Models;
using QuickCode.Turuncu.SmsManagerModule.Domain.Entities;
using QuickCode.Turuncu.SmsManagerModule.Application.Interfaces.Repositories;
using QuickCode.Turuncu.SmsManagerModule.Application.Dtos;

namespace QuickCode.Turuncu.SmsManagerModule.Application.Features
{
    public class InfoTypesGetItemQuery : IRequest<Response<InfoTypesDto>>
    {
        public int Id { get; set; }

        public InfoTypesGetItemQuery(int id)
        {
            this.Id = id;
        }

        public class InfoTypesGetItemHandler : IRequestHandler<InfoTypesGetItemQuery, Response<InfoTypesDto>>
        {
            private readonly ILogger<InfoTypesGetItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IInfoTypesRepository _repository;
            public InfoTypesGetItemHandler(IMapper mapper, ILogger<InfoTypesGetItemHandler> logger, IInfoTypesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<InfoTypesDto>> Handle(InfoTypesGetItemQuery request, CancellationToken cancellationToken)
            {
                var returnValue = _mapper.Map<Response<InfoTypesDto>>(await _repository.GetByPkAsync(request.Id));
                return returnValue;
            }
        }
    }
}