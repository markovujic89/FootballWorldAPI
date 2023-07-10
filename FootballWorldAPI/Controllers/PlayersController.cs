using Application;
using Application.DTOs.Player;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Roles = "Reader")]
        public async Task<ActionResult<List<PlayerDTO>>> GetAllPlayers([FromQuery] string? filterOn, 
            [FromQuery] string? filterQuery, 
            [FromQuery] string? sortBy, 
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            return await _playersService.GetAllPlayersAsync(filterOn, filterQuery, sortBy, isAscending?? true, pageNumber, pageSize);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Reader")]
        public async Task<ActionResult<PlayerDTO>> GetPlayerById([FromQuery] Guid id)
        {
            var playerDTO = await _playersService.GetPlayerByIdAsync(id);

            if(playerDTO is null)
            {
                return BadRequest($"Player with id: {id} doesn't exist");
            }

            return Ok(playerDTO);
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
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
        [Authorize(Roles = "Writer")]
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
        [Authorize(Roles = "Writer")]
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
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeletePlayer([FromRoute] Guid id)
        {
            await _playersService.RemovePlayerAsync(id);

            return Ok();
        }

        [HttpPost]
        [Route("api/players/{playerId}/clubs/{clubId}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> AssignePlayerToClubAsync([FromRoute] Guid playerId, [FromRoute] Guid clubId)
        {
            await _playersService.AssignePlayerToClubAsync(playerId, clubId);

            return Ok();
        }

        
    }
}
