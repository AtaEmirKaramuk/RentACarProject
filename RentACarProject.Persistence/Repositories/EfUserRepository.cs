using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Domain.Entities;
using RentACarProject.Domain.Enums;
using RentACarProject.Persistence.Context;
using System.Linq.Expressions;

namespace RentACarProject.Persistence.Repositories
{
    public class EfUserRepository : EfGenericRepository<User>, IUserRepository
    {
        public EfUserRepository(RentACarDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email != null && u.Email.ToLower() == email.ToLower());
        }

        public async Task<List<User>> GetUsersByRoleAsync(string role)
        {
            if (!Enum.TryParse<UserRole>(role, true, out var parsedRole))
                return new List<User>();

            return await _context.Users
                .Where(u => u.Role == parsedRole)
                .ToListAsync();
        }

        public async Task<User?> GetUserWithDetailsAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.Customer)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            return await _context.Users
                .Where(x => !x.IsDeleted && x.Id == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users
                .Where(x => !x.IsDeleted)
                .AnyAsync(predicate);
        }
    }
}
