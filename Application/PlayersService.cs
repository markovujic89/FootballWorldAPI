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

        public async Task AddPlayerAsync(CreatePlayerDTO createPlayerDTO)
        {
            var player = _mapper.Map<Player>(createPlayerDTO);

            await _dataContext.AddAsync(player);

            await _dataContext.SaveChangesAsync();
        }

        public async Task AssignePlayerToClubAsync(Guid playerId, Guid clubId)
        {
            var player = await _dataContext.Players.FindAsync(playerId);
            var club = await _dataContext.Clubs.FindAsync(clubId);

            if (player is null || club is null)
            {
                return;
            }

            club.Players ??= new List<Player>();

            if (!club.Players.Contains(player))
            {
                club.Players.Add(player);
            }

            player.Club = club;
            player.ClubId = clubId;

            await _dataContext.SaveChangesAsync();
        }

        public async Task EditPlayerAsync(Guid id, EditPlayerDTO editPlayerDTO)
        {
            var plyer = await _dataContext.Players.FindAsync(id);

            if (plyer is not null)
            {
                _mapper.Map(editPlayerDTO, plyer);

                await _dataContext.SaveChangesAsync();
            }
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

        public async Task RemovePlayerAsync(Guid id)
        {
            var player = await _dataContext.Players.SingleOrDefaultAsync(x => x.Id == id);

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
