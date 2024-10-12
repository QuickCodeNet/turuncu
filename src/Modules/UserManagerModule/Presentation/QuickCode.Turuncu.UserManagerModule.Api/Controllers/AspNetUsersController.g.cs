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
    public partial class AspNetUsersController : QuickCodeBaseApiController
    {
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly ILogger<AspNetUsersController> logger;
        public AspNetUsersController(IMapper mapper, IMediator mediator, ILogger<AspNetUsersController> logger)
        {
            this.mapper = mapper;
            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AspNetUsersDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ListAsync(int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await mediator.Send(new AspNetUsersListQuery(page, size));
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
            var response = await mediator.Send(new AspNetUsersTotalItemCountQuery());
            if (response.Code != 0)
            {
                var errorMessage = $"Response Code: {response.Code}, Message: {response.Message}";
                logger.LogError($"List Error: '{errorMessage}''");
                return BadRequest(errorMessage);
            }

            return Ok(response.Value);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AspNetUsersDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetItemAsync(string id)
        {
            var response = await mediator.Send(new AspNetUsersGetItemQuery(id));
            if (response.Code == 404)
            {
                var notFoundMessage = $"Id: '{id}' not found in AspNetUsers Table";
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AspNetUsersDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> InsertAsync(AspNetUsersDto model)
        {
            var response = await mediator.Send(new AspNetUsersInsertCommand(model));
            if (response.Code != 0)
            {
                var errorMessage = $"Response Code: {response.Code}, Message: {response.Message}";
                logger.LogError($"List Error: '{errorMessage}''");
                return BadRequest(errorMessage);
            }

            return CreatedAtRoute(new { id = response.Value.Id }, response.Value);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateAsync(string id, AspNetUsersDto model)
        {
            if (!(model.Id == id))
            {
                return BadRequest($"Id: '{id}' must be equal to model.Id: '{model.Id}'");
            }

            var response = await mediator.Send(new AspNetUsersUpdateCommand(id, model));
            if (response.Code == 404)
            {
                var notFoundMessage = $"Id: '{id}' not found in AspNetUsers Table";
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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var response = await mediator.Send(new AspNetUsersDeleteItemCommand(id));
            if (response.Code == 404)
            {
                var notFoundMessage = $"Id: '{id}' not found in AspNetUsers Table";
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

        [HttpGet("get-user/{aspNetUsersEmail}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AspNetUsersGetUserResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> AspNetUsersGetUserAsync(string aspNetUsersEmail)
        {
            var response = await mediator.Send(new AspNetUsersAspNetUsersGetUserQuery(aspNetUsersEmail));
            if (response.Code == 404)
            {
                var notFoundMessage = $"AspNetUsersEmail: '{aspNetUsersEmail}' not found in AspNetUsers Table";
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

        [HttpGet("{aspNetUsersId}/asp-net-user-roles")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AspNetUsersAspNetUserRoles_RESTResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> AspNetUsersAspNetUserRoles_RESTAsync(string aspNetUsersId)
        {
            var response = await mediator.Send(new AspNetUsersAspNetUsersAspNetUserRoles_RESTQuery(aspNetUsersId));
            if (response.Code == 404)
            {
                var notFoundMessage = $"AspNetUsersId: '{aspNetUsersId}' not found in AspNetUsers Table";
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

        [HttpGet("{aspNetUsersId}/asp-net-user-roles/{aspNetUserRolesUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AspNetUsersAspNetUserRoles_KEY_RESTResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> AspNetUsersAspNetUserRoles_KEY_RESTAsync(string aspNetUsersId, string aspNetUserRolesUserId)
        {
            var response = await mediator.Send(new AspNetUsersAspNetUsersAspNetUserRoles_KEY_RESTQuery(aspNetUsersId, aspNetUserRolesUserId));
            if (response.Code == 404)
            {
                var notFoundMessage = $"AspNetUsersId: '{aspNetUsersId}', AspNetUserRolesUserId: '{aspNetUserRolesUserId}' not found in AspNetUsers Table";
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

        [HttpGet("{aspNetUsersId}/asp-net-user-claims")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AspNetUsersAspNetUserClaims_RESTResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> AspNetUsersAspNetUserClaims_RESTAsync(string aspNetUsersId)
        {
            var response = await mediator.Send(new AspNetUsersAspNetUsersAspNetUserClaims_RESTQuery(aspNetUsersId));
            if (response.Code == 404)
            {
                var notFoundMessage = $"AspNetUsersId: '{aspNetUsersId}' not found in AspNetUsers Table";
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

        [HttpGet("{aspNetUsersId}/asp-net-user-claims/{aspNetUserClaimsId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AspNetUsersAspNetUserClaims_KEY_RESTResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> AspNetUsersAspNetUserClaims_KEY_RESTAsync(string aspNetUsersId, int aspNetUserClaimsId)
        {
            var response = await mediator.Send(new AspNetUsersAspNetUsersAspNetUserClaims_KEY_RESTQuery(aspNetUsersId, aspNetUserClaimsId));
            if (response.Code == 404)
            {
                var notFoundMessage = $"AspNetUsersId: '{aspNetUsersId}', AspNetUserClaimsId: '{aspNetUserClaimsId}' not found in AspNetUsers Table";
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

        [HttpGet("{aspNetUsersId}/asp-net-user-tokens")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AspNetUsersAspNetUserTokens_RESTResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> AspNetUsersAspNetUserTokens_RESTAsync(string aspNetUsersId)
        {
            var response = await mediator.Send(new AspNetUsersAspNetUsersAspNetUserTokens_RESTQuery(aspNetUsersId));
            if (response.Code == 404)
            {
                var notFoundMessage = $"AspNetUsersId: '{aspNetUsersId}' not found in AspNetUsers Table";
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

        [HttpGet("{aspNetUsersId}/asp-net-user-tokens/{aspNetUserTokensUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AspNetUsersAspNetUserTokens_KEY_RESTResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> AspNetUsersAspNetUserTokens_KEY_RESTAsync(string aspNetUsersId, string aspNetUserTokensUserId)
        {
            var response = await mediator.Send(new AspNetUsersAspNetUsersAspNetUserTokens_KEY_RESTQuery(aspNetUsersId, aspNetUserTokensUserId));
            if (response.Code == 404)
            {
                var notFoundMessage = $"AspNetUsersId: '{aspNetUsersId}', AspNetUserTokensUserId: '{aspNetUserTokensUserId}' not found in AspNetUsers Table";
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

        [HttpGet("{aspNetUsersId}/asp-net-user-logins")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AspNetUsersAspNetUserLogins_RESTResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> AspNetUsersAspNetUserLogins_RESTAsync(string aspNetUsersId)
        {
            var response = await mediator.Send(new AspNetUsersAspNetUsersAspNetUserLogins_RESTQuery(aspNetUsersId));
            if (response.Code == 404)
            {
                var notFoundMessage = $"AspNetUsersId: '{aspNetUsersId}' not found in AspNetUsers Table";
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

        [HttpGet("{aspNetUsersId}/asp-net-user-logins/{aspNetUserLoginsLoginProvider}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AspNetUsersAspNetUserLogins_KEY_RESTResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> AspNetUsersAspNetUserLogins_KEY_RESTAsync(string aspNetUsersId, string aspNetUserLoginsLoginProvider)
        {
            var response = await mediator.Send(new AspNetUsersAspNetUsersAspNetUserLogins_KEY_RESTQuery(aspNetUsersId, aspNetUserLoginsLoginProvider));
            if (response.Code == 404)
            {
                var notFoundMessage = $"AspNetUsersId: '{aspNetUsersId}', AspNetUserLoginsLoginProvider: '{aspNetUserLoginsLoginProvider}' not found in AspNetUsers Table";
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

        [HttpGet("{aspNetUsersId}/refresh-tokens")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AspNetUsersRefreshTokens_RESTResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> AspNetUsersRefreshTokens_RESTAsync(string aspNetUsersId)
        {
            var response = await mediator.Send(new AspNetUsersAspNetUsersRefreshTokens_RESTQuery(aspNetUsersId));
            if (response.Code == 404)
            {
                var notFoundMessage = $"AspNetUsersId: '{aspNetUsersId}' not found in AspNetUsers Table";
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

        [HttpGet("{aspNetUsersId}/refresh-tokens/{refreshTokensId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AspNetUsersRefreshTokens_KEY_RESTResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> AspNetUsersRefreshTokens_KEY_RESTAsync(string aspNetUsersId, int refreshTokensId)
        {
            var response = await mediator.Send(new AspNetUsersAspNetUsersRefreshTokens_KEY_RESTQuery(aspNetUsersId, refreshTokensId));
            if (response.Code == 404)
            {
                var notFoundMessage = $"AspNetUsersId: '{aspNetUsersId}', RefreshTokensId: '{refreshTokensId}' not found in AspNetUsers Table";
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