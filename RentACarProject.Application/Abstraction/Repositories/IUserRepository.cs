using RentACarProject.Domain.Entities;

namespace RentACarProject.Application.Abstraction.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByUserNameAsync(string userName);
        Task<User?> GetByEmailAsync(string email);
        Task<List<User>> GetUsersByRoleAsync(string role);
        Task<User?> GetUserWithDetailsAsync(Guid userId);
    }
}
