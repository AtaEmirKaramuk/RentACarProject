using RentACarProject.Domain.Entities;

namespace RentACarProject.Application.Abstraction.Repositories
{
    public interface ICarRepository : IGenericRepository<Car>
    {
        Task<List<Car>> GetAvailableCarsAsync();
        Task<List<Car>> GetCarsByBrandIdAsync(Guid brandId);
        Task<List<Car>> GetCarsByModelIdAsync(Guid modelId);
        Task<List<Car>> GetCarsByYearAsync(int year);
        Task<List<Car>> GetCarsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    }
}
