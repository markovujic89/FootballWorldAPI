using Application.DTOs.Player;
using FluentValidation.Results;

namespace Application
{
    public interface IPlayersService
    {
        Task<List<PlayerDTO>> GetAllPlayersAsync(string? filterOn = null,
            string? filterQuer = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 10);

        Task<PlayerDTO> GetPlayerByIdAsync(Guid id);

        Task AddPlayerAsync(CreatePlayerDTO player);

        Task<List<ValidationFailure>> BulkImport(List<CreatePlayerDTO> players);

        Task RemovePlayerAsync(Guid id);

        Task EditPlayerAsync(Guid id, EditPlayerDTO playerDTO);

        Task AssignePlayerToClubAsync(Guid playerId, Guid clubId);
    }
}
