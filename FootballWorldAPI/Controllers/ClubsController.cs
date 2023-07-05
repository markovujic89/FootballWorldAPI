using Application;
using Application.DTOs;
using Application.Validations.Player;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FootballWorldAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly IClubService _clubService;
        private readonly IValidator<CreateClubDTO> _createClubValidator;
        private readonly IValidator<EditClubDTO> _editClubValidator;

        public ClubsController(IClubService clubService, IValidator<CreateClubDTO> createClubValidator, IValidator<EditClubDTO> editClubValidator)
        {
            _clubService = clubService;
            _createClubValidator = createClubValidator;
            _editClubValidator = editClubValidator;
        }

        [HttpGet]
        public async Task<ActionResult<List<ClubDTO>>> GetAllClubs()
        {
            return await _clubService.GetAllClubsAsync();
        }

        // https://localhost:****/api/clubs/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ClubDTO>> GetById([FromRoute]Guid id)
        {
            var club = await _clubService.GetClubByIdAsync(id);

            if(club is  null)
            {
                return NotFound();
            }

            return Ok(club);
        }

        [HttpPost]
        public async Task<ActionResult> CreateClub(CreateClubDTO createClubDTO)
        {
            try
            {
                var validationResult = await _createClubValidator.ValidateAsync(createClubDTO);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                await _clubService.AddClubAsync(createClubDTO);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> EditClub([FromRoute] Guid id, [FromBody] EditClubDTO editClubDTO)
        {
            var validationResult = await _editClubValidator.ValidateAsync(editClubDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _clubService.EditClubAsync(id, editClubDTO);
            return Ok();
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteClub([FromRoute]Guid id)
        {
            await _clubService.RemoveClubAsync(id);

            return Ok();
        }
    }
}
