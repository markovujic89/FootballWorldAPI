using Application.DTOs;

namespace Application
{
    public interface IPlayersService
    {
        Task<List<PlayerDTO>> GetAllPlayersAsync(string? filterOn = null, string? filterQuer = null);

        Task<PlayerDTO> GetPlayerByIdAsync(Guid id);

        Task AddPlayerAsync(CreatePlayerDTO player);

        Task RemovePlayerAsync(Guid id);

        Task EditPlayerAsync(Guid id, EditPlayerDTO playerDTO);

        Task AssignePlayerToClubAsync(Guid playerId, Guid clubId);
    }
}
