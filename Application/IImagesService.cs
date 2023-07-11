using Domain;

namespace Application
{
    public interface IImagesService
    {
        Task<Image> Upload(Image image);
    }
}
