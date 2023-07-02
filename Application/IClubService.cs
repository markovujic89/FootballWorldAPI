using Application.DTOs;
using Domain;

namespace Application
{
    public interface IClubService
    {
        Task<List<ClubDTO>> GetAllClubsAsync();

        Task<ClubDTO> GetClubByIdAsync(Guid id);

        Task AddClubAsync(ClubDTO club);

        Task RemoveClubAsync(ClubDTO club);

        Task EditClubAsync(Guid id);
    }
}
