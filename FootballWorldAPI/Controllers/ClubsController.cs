﻿using Application;
using Application.DTOs;
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
        public async Task<ActionResult> CreateClub(ClubDTO clubDTO)
        {
            try
            {
                await _clubService.AddClubAsync(clubDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }
    }
}