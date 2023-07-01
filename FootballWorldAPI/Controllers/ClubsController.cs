using Application;
using Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FootballWorldAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly IClubService _clubService;

        public ClubsController(IClubService clubService)
        {
            _clubService = clubService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Club>>> GetAllClubs()
        {
            return await _clubService.GetAllClubsAsync();
        }

        [HttpPost]
        public async Task<ActionResult> CreateClub(Club club)
        {
            try
            {
                await _clubService.AddClubAsync(club);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }
    }
}
