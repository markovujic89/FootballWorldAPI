using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class ClubService : IClubService
    {
        private readonly DataContext _dataContext;

        public ClubService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddClubAsync(Club club)
        {
            await _dataContext.AddAsync(club);

            await _dataContext.SaveChangesAsync();
        }

        public Task EditClubAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Club>> GetAllClubsAsync()
        {
            return await _dataContext.Clubs.ToListAsync();
        }

        public Task<Club> GetClubByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClubAsync(Club club)
        {
            throw new NotImplementedException();
        }
    }
}
