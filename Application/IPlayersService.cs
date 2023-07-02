using Application.DTOs;
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
        Task<List<PlayerDTO>> GetAllPlayersAsync();

        Task<PlayerDTO> GetPlayerByIdAsync(Guid id);

        Task AddPlayerAsync(PlayerDTO player);

        Task RemovePlayerAsync(PlayerDTO player);

        Task EditPlayerAsync(Guid playerId);
    }
}
