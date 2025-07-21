using RentACarProject.Domain.Entities;
using System.Linq.Expressions;

namespace RentACarProject.Application.Abstraction.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByIdAsync(Guid userId);
        Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate);

        Task<User?> GetByUserNameAsync(string userName);
        Task<User?> GetByEmailAsync(string email);
        Task<List<User>> GetUsersByRoleAsync(string role);
        Task<User?> GetUserWithDetailsAsync(Guid userId);
    }
}
