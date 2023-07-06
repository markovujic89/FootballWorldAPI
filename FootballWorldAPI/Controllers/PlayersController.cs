﻿using Application;
using Application.DTOs;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using System.ComponentModel.DataAnnotations;

namespace FootballWorldAPI.Controllers
{
    // https://localhost:xxxx/api/players
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayersService _playersService;
        private readonly IValidator<CreatePlayerDTO> _cratePlayerValidator;
        private readonly IValidator<EditPlayerDTO> _editPlayerValidator;

        public PlayersController(IPlayersService playersService, IValidator<CreatePlayerDTO> cratePlayerValidator, IValidator<EditPlayerDTO> editPlayerValidator)
        {
            _playersService = playersService;
            _cratePlayerValidator = cratePlayerValidator;
            _editPlayerValidator = editPlayerValidator;
        }

        // GET Players
        // GET: /api/players?filterOn=Name&filterQuery=Tom
        [HttpGet]
        public async Task<ActionResult<List<PlayerDTO>>> GetAllPlayers([FromQuery] string? filterOn, 
            [FromQuery] string? filterQuery, 
            [FromQuery] string? sortBy, 
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            return await _playersService.GetAllPlayersAsync(filterOn, filterQuery, sortBy, isAscending?? true, pageNumber, pageSize);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlayer(CreatePlayerDTO playerDTO)
        {
            var validationResult = await _cratePlayerValidator.ValidateAsync(playerDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                await _playersService.AddPlayerAsync(playerDTO);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        [Route("api/players/bulkImport")]
        [HttpPost]
        public async Task<IActionResult> BulkImport(List<CreatePlayerDTO> playerDTOs)
        {
            var errors = await _playersService.BulkImport(playerDTOs);

            if(errors.Count > 0)
            {
                return StatusCode(207, errors);
            }

            return Ok("Bulk import successful");
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> EditPlayer([FromRoute] Guid id, [FromBody] EditPlayerDTO editPlayerDTO)
        {
            var validationResult = await _editPlayerValidator.ValidateAsync(editPlayerDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _playersService.EditPlayerAsync(id, editPlayerDTO);

            return Ok();
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeletePlayer([FromRoute] Guid id)
        {
            await _playersService.RemovePlayerAsync(id);

            return Ok();
        }

        [HttpPost]
        [Route("api/players/{playerId}/clubs/{clubId}")]
        public async Task<IActionResult> AssignePlayerToClubAsync([FromRoute] Guid playerId, [FromRoute] Guid clubId)
        {
            await _playersService.AssignePlayerToClubAsync(playerId, clubId);

            return Ok();
        }

        
    }
}
