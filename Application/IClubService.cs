using Application.DTOs;
using Domain;

namespace Application
{
    public interface IClubService
    {
        Task<List<ClubDTO>> GetAllClubsAsync();

        Task<ClubDTO> GetClubByIdAsync(Guid id);

        Task AddClubAsync(CreateClubDTO createClubDTO);

        Task RemoveClubAsync(ClubDTO clubDTO);

        Task EditClubAsync(Guid id, EditClubDTO clubDTO);
    }
}
