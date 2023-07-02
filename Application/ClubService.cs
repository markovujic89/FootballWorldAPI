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

        public async Task AddClubAsync(ClubDTO clubDTO)
        {
            var club = _mapper.Map<Club>(clubDTO);
            
            await _dataContext.AddAsync(club);

            await _dataContext.SaveChangesAsync();
        }

        public Task EditClubAsync(Guid id)
        {
            throw new NotImplementedException();
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

        public Task RemoveClubAsync(ClubDTO club)
        {
            throw new NotImplementedException();
        }
    }
}
