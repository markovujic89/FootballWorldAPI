using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IPlayersService
    {
        Task<List<Player>> GetAllPlayersAsync();

        Task<Player> GetPlayerByIdAsync(Guid id);

        Task AddPlayerAsync(Player player);

        Task RemovePlayerAsync(Player player);

        Task EditPlayerAsync(Guid playerId);
    }
}
