using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Persistence;

namespace Application
{
    public class ImagesService : IImagesService
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImagesService(DataContext dataContext, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");

            // upload Image to Local Path
            using(var stream = new FileStream(localFilePath, FileMode.Create))
            {
                await image.File.CopyToAsync(stream);
            }

            // https://localhost:5500/images/images.jpg
            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/{_httpContextAccessor.HttpContext.Request.PathBase}Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            // Add image to images table

            await _dataContext.Images.AddAsync(image);
            await _dataContext.SaveChangesAsync();

            return image;
        }
    }
}
