using Application.DTOs;
using AutoMapper;
using Azure.Core;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Diagnostics;

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

        public async Task EditClubAsync(Guid id,EditClubDTO editClubDTO)
        {
            var club = await _dataContext.Clubs.FindAsync(id);

            if(club is not null)
            {
                club = _mapper.Map<Club>(editClubDTO);

                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task<List<ClubDTO>> GetAllClubsAsync()
        {
            var clubs = await _dataContext.Clubs.ToListAsync();
            var clubsDTO = _mapper.Map<List<ClubDTO>>(clubs);

            return clubsDTO;
        }

        public async Task<ClubDTO> GetClubByIdAsync(Guid id)
        {
            var club = await _dataContext.Clubs.FindAsync(id);

            var clubDTO = _mapper.Map<ClubDTO>(club);

            return clubDTO;
        }

        public async Task RemoveClubAsync(ClubDTO clubDTO)
        {
            var club = _dataContext.Clubs.Find(clubDTO.Id);

            if(club is not null)
            {
                _dataContext.Clubs.Remove(club);

                await _dataContext.SaveChangesAsync();
            }
        }
    }
}
