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
    public class EmailSendersDeleteItemCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }

        public EmailSendersDeleteItemCommand(int id)
        {
            this.Id = id;
        }

        public class EmailSendersDeleteItemHandler : IRequestHandler<EmailSendersDeleteItemCommand, Response<bool>>
        {
            private readonly ILogger<EmailSendersDeleteItemHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IEmailSendersRepository _repository;
            public EmailSendersDeleteItemHandler(IMapper mapper, ILogger<EmailSendersDeleteItemHandler> logger, IEmailSendersRepository repository)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(EmailSendersDeleteItemCommand request, CancellationToken cancellationToken)
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