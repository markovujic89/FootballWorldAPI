using Application;
using Application.DTOs.Image;
using Application.DTOs.Player;
using Application.Validations.Images;
using Application.Validations.Player;
using Domain;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FootballWorldAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController: ControllerBase
    {
        private readonly IValidator<ImageUploadRequestDTO> _imageUploadRequestValidator;
        private readonly IImagesService _imagesService;

        public ImagesController(IValidator<ImageUploadRequestDTO> imageUploadRequestValidator, IImagesService imagesService)
        {
            _imageUploadRequestValidator = imageUploadRequestValidator;
            _imagesService = imagesService;
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

            var image = new Image
            {
                File = imageUploadRequestDTO.File,
                FileExtension = Path.GetExtension(imageUploadRequestDTO.File.FileName),
                FileSizeInBytes = imageUploadRequestDTO.File.Length,
                FileName = imageUploadRequestDTO.FileName,
                FileDescription = imageUploadRequestDTO.FileDescription
            };

            await _imagesService.Upload(image);


            return Ok(image);
        }
    }
}
