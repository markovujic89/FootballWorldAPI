using Application.DTOs;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class PlayersService : IPlayersService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public PlayersService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task AddPlayerAsync(PlayerDTO playerDTO)
        {
            var player = _mapper.Map<Player>(playerDTO);

            await _dataContext.AddAsync(player);

            await _dataContext.SaveChangesAsync();
        }

        public Task EditPlayerAsync(Guid playerId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PlayerDTO>> GetAllPlayersAsync()
        {
            var players = await _dataContext.Players.ToListAsync();

            var playersDTO = _mapper.Map<List<PlayerDTO>>(players);

            return playersDTO;
        }

        public async Task<PlayerDTO> GetPlayerByIdAsync(Guid id)
        {
            var player = await _dataContext.Players.SingleOrDefaultAsync();

            if (player == null)
            {
                return await Task.FromResult<PlayerDTO>(null);
            }

            var playerDTO = _mapper.Map<PlayerDTO>(player);

            return playerDTO;
        }

        public async Task RemovePlayerAsync(PlayerDTO playerDTO)
        {
            var player = await _dataContext.Players.SingleOrDefaultAsync(x=>x.Id == playerDTO.Id);

            if (player is null)
            {
                // log
                return;
            }
 
            _dataContext.Players.Remove(player);

            await _dataContext.SaveChangesAsync();
        }
    }
}
