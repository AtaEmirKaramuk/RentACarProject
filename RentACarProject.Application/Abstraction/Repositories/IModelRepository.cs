using RentACarProject.Domain.Entities;

namespace RentACarProject.Application.Abstraction.Repositories
{
    public interface IModelRepository : IGenericRepository<Model>
    {
        Task<List<Model>> GetModelsByBrandIdAsync(Guid brandId);
    }
}
