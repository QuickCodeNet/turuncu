using AutoMapper;
using System.Linq;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.Turuncu.EmailManagerModule.Application.Models;
using QuickCode.Turuncu.EmailManagerModule.Domain.Entities;
using QuickCode.Turuncu.EmailManagerModule.Application.Interfaces.Repositories;
using QuickCode.Turuncu.EmailManagerModule.Application.Dtos;

namespace QuickCode.Turuncu.EmailManagerModule.Application.Features
{
    public class InfoMessagesDeleteCommand : IRequest<Response<bool>>
    {
        public InfoMessagesDto request { get; set; }

        public InfoMessagesDeleteCommand(InfoMessagesDto request)
        {
            this.request = request;
        }

        public class InfoMessagesDeleteHandler : IRequestHandler<InfoMessagesDeleteCommand, Response<bool>>
        {
            private readonly ILogger<InfoMessagesDeleteHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IInfoMessagesRepository _repository;
            public InfoMessagesDeleteHandler(IMapper mapper, ILogger<InfoMessagesDeleteHandler> logger, IInfoMessagesRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(InfoMessagesDeleteCommand request, CancellationToken cancellationToken)
            {
                var model = _mapper.Map<InfoMessages>(request.request);
                var returnValue = _mapper.Map<Response<bool>>(await _repository.DeleteAsync(model));
                return returnValue;
            }
        }
    }
}