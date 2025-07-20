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

        // Eğer ekstra metot varsa buraya eklenir
        public async Task<List<Location>> GetAllLocationsAsync()
        {
            return await Query().ToListAsync();
        }
    }
}
