using AutoMapper;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuickCode.Turuncu.Common.Controllers;
using QuickCode.Turuncu.UserManagerModule.Application.Dtos;
using QuickCode.Turuncu.UserManagerModule.Application.Features;

namespace QuickCode.Turuncu.UserManagerModule.Api.Controllers
{
    public partial class KafkaEventsController : QuickCodeBaseApiController
    {
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly ILogger<KafkaEventsController> logger;
        public KafkaEventsController(IMapper mapper, IMediator mediator, ILogger<KafkaEventsController> logger)
        {
            this.mapper = mapper;
            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<KafkaEventsDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ListAsync(int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await mediator.Send(new KafkaEventsListQuery(page, size));
            if (response.Code != 0)
            {
                var errorMessage = $"Response Code: {response.Code}, Message: {response.Message}";
                logger.LogError($"List Error: '{errorMessage}''");
                return BadRequest(errorMessage);
            }

            return Ok(response.Value);
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> TotalItemCountAsync()
        {
            var response = await mediator.Send(new KafkaEventsTotalItemCountQuery());
            if (response.Code != 0)
            {
                var errorMessage = $"Response Code: {response.Code}, Message: {response.Message}";
                logger.LogError($"List Error: '{errorMessage}''");
                return BadRequest(errorMessage);
            }

            return Ok(response.Value);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(KafkaEventsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetItemAsync(int id)
        {
            var response = await mediator.Send(new KafkaEventsGetItemQuery(id));
            if (response.Code == 404)
            {
                var notFoundMessage = $"Id: '{id}' not found in KafkaEvents Table";
                logger.LogWarning($"List Error: '{notFoundMessage}''");
                return NotFound(notFoundMessage);
            }
            else if (response.Code != 0)
            {
                var errorMessage = $"Response Code: {response.Code}, Message: {response.Message}";
                logger.LogError($"List Error: '{errorMessage}''");
                return BadRequest(errorMessage);
            }

            return Ok(response.Value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(KafkaEventsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> InsertAsync(KafkaEventsDto model)
        {
            var response = await mediator.Send(new KafkaEventsInsertCommand(model));
            if (response.Code != 0)
            {
                var errorMessage = $"Response Code: {response.Code}, Message: {response.Message}";
                logger.LogError($"List Error: '{errorMessage}''");
                return BadRequest(errorMessage);
            }

            return CreatedAtRoute(new { id = response.Value.Id }, response.Value);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateAsync(int id, KafkaEventsDto model)
        {
            if (!(model.Id == id))
            {
                return BadRequest($"Id: '{id}' must be equal to model.Id: '{model.Id}'");
            }

            var response = await mediator.Send(new KafkaEventsUpdateCommand(id, model));
            if (response.Code == 404)
            {
                var notFoundMessage = $"Id: '{id}' not found in KafkaEvents Table";
                logger.LogWarning($"List Error: '{notFoundMessage}''");
                return NotFound(notFoundMessage);
            }
            else if (response.Code != 0)
            {
                var errorMessage = $"Response Code: {response.Code}, Message: {response.Message}";
                logger.LogError($"List Error: '{errorMessage}''");
                return BadRequest(errorMessage);
            }

            return Ok(response.Value);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await mediator.Send(new KafkaEventsDeleteItemCommand(id));
            if (response.Code == 404)
            {
                var notFoundMessage = $"Id: '{id}' not found in KafkaEvents Table";
                logger.LogWarning($"List Error: '{notFoundMessage}''");
                return NotFound(notFoundMessage);
            }
            else if (response.Code != 0)
            {
                var errorMessage = $"Response Code: {response.Code}, Message: {response.Message}";
                logger.LogError($"List Error: '{errorMessage}''");
                return BadRequest(errorMessage);
            }

            return Ok(response.Value);
        }

        [HttpGet("get-kafka-events")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<KafkaEventsGetKafkaEventsResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> KafkaEventsGetKafkaEventsAsync()
        {
            var response = await mediator.Send(new KafkaEventsKafkaEventsGetKafkaEventsQuery());
            if (response.Code == 404)
            {
                var notFoundMessage = $" not found in KafkaEvents Table";
                logger.LogWarning($"List Error: '{notFoundMessage}''");
                return NotFound(notFoundMessage);
            }
            else if (response.Code != 0)
            {
                var errorMessage = $"Response Code: {response.Code}, Message: {response.Message}";
                logger.LogError($"List Error: '{errorMessage}''");
                return BadRequest(errorMessage);
            }

            return Ok(response.Value);
        }

        [HttpGet("get-active-kafka-events")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<KafkaEventsGetActiveKafkaEventsResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> KafkaEventsGetActiveKafkaEventsAsync()
        {
            var response = await mediator.Send(new KafkaEventsKafkaEventsGetActiveKafkaEventsQuery());
            if (response.Code == 404)
            {
                var notFoundMessage = $" not found in KafkaEvents Table";
                logger.LogWarning($"List Error: '{notFoundMessage}''");
                return NotFound(notFoundMessage);
            }
            else if (response.Code != 0)
            {
                var errorMessage = $"Response Code: {response.Code}, Message: {response.Message}";
                logger.LogError($"List Error: '{errorMessage}''");
                return BadRequest(errorMessage);
            }

            return Ok(response.Value);
        }

        [HttpGet("{kafkaEventsId}/topic-workflows")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<KafkaEventsTopicWorkflows_RESTResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> KafkaEventsTopicWorkflows_RESTAsync(int kafkaEventsId)
        {
            var response = await mediator.Send(new KafkaEventsKafkaEventsTopicWorkflows_RESTQuery(kafkaEventsId));
            if (response.Code == 404)
            {
                var notFoundMessage = $"KafkaEventsId: '{kafkaEventsId}' not found in KafkaEvents Table";
                logger.LogWarning($"List Error: '{notFoundMessage}''");
                return NotFound(notFoundMessage);
            }
            else if (response.Code != 0)
            {
                var errorMessage = $"Response Code: {response.Code}, Message: {response.Message}";
                logger.LogError($"List Error: '{errorMessage}''");
                return BadRequest(errorMessage);
            }

            return Ok(response.Value);
        }

        [HttpGet("{kafkaEventsId}/topic-workflows/{topicWorkflowsId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(KafkaEventsTopicWorkflows_KEY_RESTResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> KafkaEventsTopicWorkflows_KEY_RESTAsync(int kafkaEventsId, int topicWorkflowsId)
        {
            var response = await mediator.Send(new KafkaEventsKafkaEventsTopicWorkflows_KEY_RESTQuery(kafkaEventsId, topicWorkflowsId));
            if (response.Code == 404)
            {
                var notFoundMessage = $"KafkaEventsId: '{kafkaEventsId}', TopicWorkflowsId: '{topicWorkflowsId}' not found in KafkaEvents Table";
                logger.LogWarning($"List Error: '{notFoundMessage}''");
                return NotFound(notFoundMessage);
            }
            else if (response.Code != 0)
            {
                var errorMessage = $"Response Code: {response.Code}, Message: {response.Message}";
                logger.LogError($"List Error: '{errorMessage}''");
                return BadRequest(errorMessage);
            }

            return Ok(response.Value);
        }
    }
}