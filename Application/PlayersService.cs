﻿using Application.DTOs.Player;
using AutoMapper;
using Domain;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;

namespace Application
{
    public class PlayersService : IPlayersService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IValidator<CreatePlayerDTO> _cratePlayerValidator;
        private readonly IConfiguration _configuration;


        public PlayersService(DataContext dataContext, IMapper mapper, IValidator<CreatePlayerDTO> cratePlayerValidator, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _cratePlayerValidator = cratePlayerValidator;
            _configuration = configuration;
        }

        public async Task AddPlayerAsync(CreatePlayerDTO createPlayerDTO)
        {
            var player = _mapper.Map<Player>(createPlayerDTO);

            await _dataContext.AddAsync(player);

            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<ValidationFailure>> BulkImport(List<CreatePlayerDTO> createPlayersDTO)
        {
            var errors = new List<ValidationFailure>();
            var tasks = new List<Task>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            foreach (var createPlayerDTO in createPlayersDTO)
            {
                tasks.Add(Task.Run(async () =>
                {
                    using (var dbContext = new DataContext(new DbContextOptionsBuilder<DataContext>().UseSqlServer(connectionString).Options))
                    {
                        var validationResult = await _cratePlayerValidator.ValidateAsync(createPlayerDTO);
                        if (!validationResult.IsValid)
                        {
                            errors.AddRange(validationResult.Errors);
                        }
                        else
                        {
                            var player = _mapper.Map<Player>(createPlayerDTO);

                            await dbContext.AddAsync(player);
                            await dbContext.SaveChangesAsync();
                        }
                    }
                }));
            }

            await Task.WhenAll(tasks);

            return errors;
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
            player.DateModified = DateTime.UtcNow;

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

        public async Task<List<PlayerDTO>> GetAllPlayersAsync(string? filterOn = null,
            string? filterQuer = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var players = _dataContext.Players.AsQueryable();

            // filtering
            if (!String.IsNullOrWhiteSpace(filterOn) && !String.IsNullOrWhiteSpace(filterQuer))
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    players = players.Where(x => x.Name.Contains(filterQuer));
                }
            }

            // sorting
            if (!String.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    players = isAscending ? players.OrderBy(x => x.Name) : players.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Age", StringComparison.OrdinalIgnoreCase))
                {
                    players = isAscending ? players.OrderBy(x => x.Age) : players.OrderByDescending(x => x.Age);
                }
            }

            // pagination
            var skipResults = (pageNumber - 1) * pageSize;

            await players.ToListAsync();

            var playersDTO = _mapper.Map<List<PlayerDTO>>(players);

            return playersDTO.Skip(skipResults).Take(pageSize).ToList();
        }

        public async Task<PlayerDTO> GetPlayerByIdAsync(Guid id)
        {
            var player = await _dataContext.Players.FirstOrDefaultAsync(x=>x.Id == id);

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
