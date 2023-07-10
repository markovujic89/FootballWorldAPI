using Application.DTOs.Image;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Application.Validations.Images
{
    public class ImageUploadRequestValidator : AbstractValidator<ImageUploadRequestDTO>
    {
        public ImageUploadRequestValidator() 
        {
            RuleFor(x => x.File)
           .NotNull().WithMessage("File is required.").
           Must(BeAValidFileExtension).WithMessage("Invalid file extension.").
           Must(file => file == null || (file.Length <= 10485760 ? true : false))
           .WithMessage("File size exceeds the maximum limit of 10MB.");

            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("FileName is required.");

            RuleFor(x => x.FileDescription)
                .NotEmpty().WithMessage("FileDescription is required.");
        }

        private bool BeAValidFileExtension(IFormFile file)
        {
            if (file != null)
            {
                var allowedExtensions = new[] { ".jpg", ".png", ".svg" };
                var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();
                return allowedExtensions.Contains(fileExtension);
            }

            return true;
        }
    }
}
