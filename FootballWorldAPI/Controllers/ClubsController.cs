using Application;
using Application.DTOs.Club;
using Application.DTOs.Player;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger<ClubsController> _logger;

        public ClubsController(IClubService clubService, IValidator<CreateClubDTO> createClubValidator, IValidator<EditClubDTO> editClubValidator, ILogger<ClubsController> logger)
        {
            _clubService = clubService;
            _createClubValidator = createClubValidator;
            _editClubValidator = editClubValidator;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<ActionResult<List<ClubDTO>>> GetAllClubs([FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            return await _clubService.GetAllClubsAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);
        }

        // https://localhost:****/api/clubs/{id}
        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Reader")]
        public async Task<ActionResult<ClubDTO>> GetById([FromRoute] Guid id)
        {
            var club = await _clubService.GetClubByIdAsync(id);

            if (club is null)
            {
                _logger.LogInformation($"Club with id: {id} doesn't exist");
                return NotFound();
            }

            return Ok(club);
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<ActionResult> CreateClub(CreateClubDTO createClubDTO)
        {
            try
            {
                var validationResult = await _createClubValidator.ValidateAsync(createClubDTO);

                if (!validationResult.IsValid)
                {
                    _logger.LogError($"Error occured during cration club: {createClubDTO.Name}, reason: {validationResult.Errors[0]}");
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
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> EditClub([FromRoute] Guid id, [FromBody] EditClubDTO editClubDTO)
        {
            var validationResult = await _editClubValidator.ValidateAsync(editClubDTO);

            if (!validationResult.IsValid)
            {
                _logger.LogError($"Error occured during updating club with id: {id}, reason: {validationResult.Errors[0]}");
                return BadRequest(validationResult.Errors);
            }

            await _clubService.EditClubAsync(id, editClubDTO);
            return Ok();
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteClub([FromRoute] Guid id)
        {
            await _clubService.RemoveClubAsync(id);

            return Ok();
        }
    }
}
