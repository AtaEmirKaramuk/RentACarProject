using RentACarProject.Domain.Entities;

namespace RentACarProject.Application.Abstraction.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer?> GetByUserIdAsync(Guid userId);
        Task<Customer?> GetByEmailAsync(string email);
        Task<Customer?> GetByPhoneAsync(string phone);
    }
}
