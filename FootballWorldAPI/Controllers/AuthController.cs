using Application;
using Application.DTOs;
using Application.DTOs.LoginAndRegistration;
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
        private readonly IValidator<LoginRequestDTO> _requestLoginValidator;
        private readonly ITokenService _tokenService;

        public AuthController(IValidator<RequestRegisterDTO> requestRegisterValidator, UserManager<IdentityUser> userManager, IValidator<LoginRequestDTO> requestLoginValidator, ITokenService tokenService)
        {
            _requestRegisterValidator = requestRegisterValidator;
            _userManager = userManager;
            _requestLoginValidator = requestLoginValidator;
            _tokenService = tokenService;
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

        // POST: /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var validationResult = await _requestLoginValidator.ValidateAsync(loginRequestDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = await _userManager.FindByEmailAsync(loginRequestDTO.UserName);

            if(user is not null)
            {
                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

                if(isPasswordCorrect)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if(roles is not null)
                    {
                        var jwtToken = _tokenService.CreateJWTToken(user, roles.ToList());
                        var loginReponseDTO = new LoginResponseDTO { JwtToken = jwtToken };

                        return Ok(loginReponseDTO);
                    }
                }
            }

            return BadRequest("User does not exist in the system. Pleas register user first!!");
        }
    }
}
