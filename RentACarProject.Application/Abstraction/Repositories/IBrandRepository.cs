using RentACarProject.Domain.Entities;

namespace RentACarProject.Application.Abstraction.Repositories
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task<Brand?> GetByNameAsync(string name);
    }
}
