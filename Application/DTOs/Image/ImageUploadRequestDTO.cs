using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Image
{
    public class ImageUploadRequestDTO
    {
        public IFormFile File { get; set; }

        public string FileName { get; set; }

        public string? FileDescription { get; set; }

    }
}
