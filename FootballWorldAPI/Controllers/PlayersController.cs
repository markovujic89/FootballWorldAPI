using Application;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FootballWorldAPI.Controllers
{
    // https://localhost:xxxx/api/players
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayersService _playersService;

        public PlayersController(IPlayersService playersService)
        {
            _playersService = playersService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PlayerDTO>>> GetAllPlayers()
        {
            return await _playersService.GetAllPlayersAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlayer(CreatePlayerDTO playerDTO)
        {
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

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> EditPlayer([FromRoute] Guid id,[FromBody] EditPlayerDTO editPlayerDTO)
        {
            await _playersService.EditPlayerAsync(id ,editPlayerDTO);

            return Ok();
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeletePlayer([FromRoute]Guid id)
        {
            await _playersService.RemovePlayerAsync(id);

            return Ok();
        }

        [HttpPost]
        [Route("api/players/{playerId}/clubs/{clubId}")]
        public async Task<IActionResult> AssignePlayerToClubAsync([FromRoute] Guid playerId, [FromRoute] Guid clubId)
        {
            await _playersService.AssignePlayerToClubAsync(playerId , clubId);

            return Ok();
        }
    }
}
