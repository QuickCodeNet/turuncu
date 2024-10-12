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
    public partial class ApiMethodDefinitionsController : QuickCodeBaseApiController
    {
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly ILogger<ApiMethodDefinitionsController> logger;
        public ApiMethodDefinitionsController(IMapper mapper, IMediator mediator, ILogger<ApiMethodDefinitionsController> logger)
        {
            this.mapper = mapper;
            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ApiMethodDefinitionsDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ListAsync(int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await mediator.Send(new ApiMethodDefinitionsListQuery(page, size));
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
            var response = await mediator.Send(new ApiMethodDefinitionsTotalItemCountQuery());
            if (response.Code != 0)
            {
                var errorMessage = $"Response Code: {response.Code}, Message: {response.Message}";
                logger.LogError($"List Error: '{errorMessage}''");
                return BadRequest(errorMessage);
            }

            return Ok(response.Value);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiMethodDefinitionsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetItemAsync(int id)
        {
            var response = await mediator.Send(new ApiMethodDefinitionsGetItemQuery(id));
            if (response.Code == 404)
            {
                var notFoundMessage = $"Id: '{id}' not found in ApiMethodDefinitions Table";
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiMethodDefinitionsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> InsertAsync(ApiMethodDefinitionsDto model)
        {
            var response = await mediator.Send(new ApiMethodDefinitionsInsertCommand(model));
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
        public async Task<IActionResult> UpdateAsync(int id, ApiMethodDefinitionsDto model)
        {
            if (!(model.Id == id))
            {
                return BadRequest($"Id: '{id}' must be equal to model.Id: '{model.Id}'");
            }

            var response = await mediator.Send(new ApiMethodDefinitionsUpdateCommand(id, model));
            if (response.Code == 404)
            {
                var notFoundMessage = $"Id: '{id}' not found in ApiMethodDefinitions Table";
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
            var response = await mediator.Send(new ApiMethodDefinitionsDeleteItemCommand(id));
            if (response.Code == 404)
            {
                var notFoundMessage = $"Id: '{id}' not found in ApiMethodDefinitions Table";
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

        [HttpGet("{apiMethodDefinitionsId}/kafka-events")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ApiMethodDefinitionsKafkaEvents_RESTResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ApiMethodDefinitionsKafkaEvents_RESTAsync(int apiMethodDefinitionsId)
        {
            var response = await mediator.Send(new ApiMethodDefinitionsApiMethodDefinitionsKafkaEvents_RESTQuery(apiMethodDefinitionsId));
            if (response.Code == 404)
            {
                var notFoundMessage = $"ApiMethodDefinitionsId: '{apiMethodDefinitionsId}' not found in ApiMethodDefinitions Table";
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

        [HttpGet("{apiMethodDefinitionsId}/kafka-events/{kafkaEventsId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiMethodDefinitionsKafkaEvents_KEY_RESTResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ApiMethodDefinitionsKafkaEvents_KEY_RESTAsync(int apiMethodDefinitionsId, int kafkaEventsId)
        {
            var response = await mediator.Send(new ApiMethodDefinitionsApiMethodDefinitionsKafkaEvents_KEY_RESTQuery(apiMethodDefinitionsId, kafkaEventsId));
            if (response.Code == 404)
            {
                var notFoundMessage = $"ApiMethodDefinitionsId: '{apiMethodDefinitionsId}', KafkaEventsId: '{kafkaEventsId}' not found in ApiMethodDefinitions Table";
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

        [HttpGet("{apiMethodDefinitionsId}/api-permission-groups")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ApiMethodDefinitionsApiPermissionGroups_RESTResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ApiMethodDefinitionsApiPermissionGroups_RESTAsync(int apiMethodDefinitionsId)
        {
            var response = await mediator.Send(new ApiMethodDefinitionsApiMethodDefinitionsApiPermissionGroups_RESTQuery(apiMethodDefinitionsId));
            if (response.Code == 404)
            {
                var notFoundMessage = $"ApiMethodDefinitionsId: '{apiMethodDefinitionsId}' not found in ApiMethodDefinitions Table";
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

        [HttpGet("{apiMethodDefinitionsId}/api-permission-groups/{apiPermissionGroupsId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiMethodDefinitionsApiPermissionGroups_KEY_RESTResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ApiMethodDefinitionsApiPermissionGroups_KEY_RESTAsync(int apiMethodDefinitionsId, int apiPermissionGroupsId)
        {
            var response = await mediator.Send(new ApiMethodDefinitionsApiMethodDefinitionsApiPermissionGroups_KEY_RESTQuery(apiMethodDefinitionsId, apiPermissionGroupsId));
            if (response.Code == 404)
            {
                var notFoundMessage = $"ApiMethodDefinitionsId: '{apiMethodDefinitionsId}', ApiPermissionGroupsId: '{apiPermissionGroupsId}' not found in ApiMethodDefinitions Table";
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