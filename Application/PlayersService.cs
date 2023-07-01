using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class PlayersService : IPlayersService
    {
        private readonly DataContext _dataContext;

        public PlayersService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddPlayerAsync(Player player)
        {
            var club = await _dataContext.Clubs.FirstOrDefaultAsync(x=>x.Id == player.ClubId);

            if (club == null)
            {
                player.Club = null;
            }

            player.Club = club;

            await _dataContext.AddAsync(player);

            await _dataContext.SaveChangesAsync();
        }

        public Task EditPlayerAsync(Guid playerId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await _dataContext.Players.ToListAsync();
        }

        public async Task<Player> GetPlayerByIdAsync(Guid id)
        {
            var player = await _dataContext.Players.SingleOrDefaultAsync();

            if (player == null)
            {
                return await Task.FromResult<Player>(null);
            }

            return player;
        }

        public async Task RemovePlayerAsync(Player player)
        {
            var _player = await _dataContext.Players.SingleOrDefaultAsync();

            if (_player is null)
            {
                // log
                return;
            }

            _dataContext.Players.Remove(player);

            await _dataContext.SaveChangesAsync();
        }
    }
}
