using Application;
using Application.DTOs;
using Domain;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> CreatePlayer(PlayerDTO playerDTO)
        {
            try
            {
                await _playersService.AddPlayerAsync(playerDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            
        }
    }
}
