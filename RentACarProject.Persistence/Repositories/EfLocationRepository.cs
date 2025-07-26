using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Domain.Entities;
using RentACarProject.Persistence.Context;

namespace RentACarProject.Persistence.Repositories
{
    public class EfLocationRepository : EfGenericRepository<Location>, ILocationRepository
    {
        public EfLocationRepository(RentACarDbContext context) : base(context)
        {
        }

        public async Task<List<Location>> GetAllLocationsAsync()
        {
            return await Query().ToListAsync();
        }

        public async Task<Location?> GetLocationByIdAsync(Guid locationId)
        {
            return await Query().FirstOrDefaultAsync(l => l.Id == locationId);
        }
    }
}
