using Application.DTOs;
using Application.Validations.Player;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FootballWorldAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //POST: /api/Auth/Register

        private readonly IValidator<RequestRegisterDTO> _requestRegisterValidator;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(IValidator<RequestRegisterDTO> requestRegisterValidator, UserManager<IdentityUser> userManager)
        {
            _requestRegisterValidator = requestRegisterValidator;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RequestRegisterDTO requestRegisterDTO)
        {
            var validationResult = await _requestRegisterValidator.ValidateAsync(requestRegisterDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var identityUser = new IdentityUser
            {
                UserName = requestRegisterDTO.UserName,
                Email = requestRegisterDTO.UserName
            };

            var identityResult = await _userManager.CreateAsync(identityUser, requestRegisterDTO.Password);

            if (identityResult.Succeeded)
            {
                identityResult = await _userManager.AddToRolesAsync(identityUser, requestRegisterDTO.Roles);

                if(identityResult.Succeeded)
                {
                    return Ok("User was registered! Pleas login!!");
                }
                
            }

            return BadRequest("Unsuccessfull registration process");
        }
    }
}
