using Application.DTOs;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ClubService : IClubService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public ClubService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task AddClubAsync(CreateClubDTO createClubDTO)
        {
            var club = _mapper.Map<Club>(createClubDTO);

            await _dataContext.AddAsync(club);

            await _dataContext.SaveChangesAsync();
        }

        public async Task EditClubAsync(Guid id, EditClubDTO editClubDTO)
        {
            var club = await _dataContext.Clubs.FindAsync(id);

            if (club is not null)
            {
                _mapper.Map(editClubDTO, club);

                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task<List<ClubDTO>> GetAllClubsAsync(string? filterOn = null,
            string? filterQuer = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 10)
        {


            var clubs = _dataContext.Clubs.AsQueryable();

            // filtering
            if (!String.IsNullOrWhiteSpace(filterOn) && !String.IsNullOrWhiteSpace(filterQuer))
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    clubs = clubs.Where(x => x.Name.Contains(filterQuer));
                }
            }


            // sorting
            if (!String.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    clubs = isAscending ? clubs.OrderBy(x => x.Name) : clubs.OrderByDescending(x => x.Name);
                }
            }

            // pagination
            var skipResults = (pageNumber - 1) * pageSize;

            await clubs.ToListAsync();

            var clubsDTO = _mapper.Map<List<ClubDTO>>(clubs);

            return clubsDTO.Skip(skipResults).Take(pageSize).ToList();
        }

        public async Task<ClubDTO> GetClubByIdAsync(Guid id)
        {
            var club = await _dataContext.Clubs.FindAsync(id);

            var clubDTO = _mapper.Map<ClubDTO>(club);

            return clubDTO;
        }

        public async Task RemoveClubAsync(Guid id)
        {
            var club = _dataContext.Clubs.Find(id);

            if (club is not null)
            {
                _dataContext.Clubs.Remove(club);

                await _dataContext.SaveChangesAsync();
            }
        }
    }
}
