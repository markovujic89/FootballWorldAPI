using Application.DTOs.Image;
using Application.DTOs.Player;
using Application.Validations.Images;
using Application.Validations.Player;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FootballWorldAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController: ControllerBase
    {
        private readonly IValidator<ImageUploadRequestDTO> _imageUploadRequestValidator;

        public ImagesController(IValidator<ImageUploadRequestDTO> imageUploadRequestValidator)
        {
            _imageUploadRequestValidator = imageUploadRequestValidator;
        }

        // Post: /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDTO imageUploadRequestDTO)
        {
            var validationResult = await _imageUploadRequestValidator.ValidateAsync(imageUploadRequestDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            return Ok();
        }
    }
}
