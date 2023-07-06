using Application.DTOs;

namespace Application
{
    public interface IClubService
    {
        Task<List<ClubDTO>> GetAllClubsAsync(string? filterOn = null,
            string? filterQuer = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ClubDTO> GetClubByIdAsync(Guid id);

        Task AddClubAsync(CreateClubDTO createClubDTO);

        Task RemoveClubAsync(Guid id);

        Task EditClubAsync(Guid id, EditClubDTO clubDTO);
    }
}
