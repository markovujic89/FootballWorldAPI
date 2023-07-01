using Domain;

namespace Application
{
    public interface IClubService
    {
        Task<List<Club>> GetAllClubsAsync();

        Task<Club> GetClubByIdAsync(Guid id);

        Task AddClubAsync(Club club);

        Task RemoveClubAsync(Club club);

        Task EditClubAsync(Guid id);
    }
}
