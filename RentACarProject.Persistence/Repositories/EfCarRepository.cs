using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Domain.Entities;
using RentACarProject.Domain.Enums; // ✅ Enum ekle
using RentACarProject.Persistence.Context;

namespace RentACarProject.Persistence.Repositories
{
    public class EfCarRepository : EfGenericRepository<Car>, ICarRepository
    {
        public EfCarRepository(RentACarDbContext context) : base(context)
        {
        }

        public async Task<List<Car>> GetAvailableCarsAsync()
        {
            return await _context.Cars
                .Where(c => c.Status == CarStatus.Available) 
                .ToListAsync();
        }

        public async Task<List<Car>> GetCarsByBrandIdAsync(Guid brandId)
        {
            return await _context.Cars
                .Include(c => c.Model)
                .Where(c => c.Model.BrandId == brandId)
                .ToListAsync();
        }

        public async Task<List<Car>> GetCarsByModelIdAsync(Guid modelId)
        {
            return await _context.Cars
                .Where(c => c.ModelId == modelId)
                .ToListAsync();
        }

        public async Task<List<Car>> GetCarsByYearAsync(int year)
        {
            return await _context.Cars
                .Where(c => c.Year == year)
                .ToListAsync();
        }

        public async Task<List<Car>> GetCarsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Cars
                .Where(c => c.DailyPrice >= minPrice && c.DailyPrice <= maxPrice)
                .ToListAsync();
        }
    }
}
