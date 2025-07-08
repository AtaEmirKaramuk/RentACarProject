using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Domain.Entities;
using RentACarProject.Persistence.Context;

namespace RentACarProject.Persistence.Repositories
{
    public class EfModelRepository : EfGenericRepository<Model>, IModelRepository
    {
        public EfModelRepository(RentACarDbContext context) : base(context)
        {
        }

        public async Task<List<Model>> GetModelsByBrandIdAsync(Guid brandId)
        {
            return await _context.Models
                .Where(m => m.BrandId == brandId)
                .Include(m => m.Brand) // Marka bilgileri de gelsin
                .ToListAsync();
        }
    }
}
