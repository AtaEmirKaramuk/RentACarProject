using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Domain.Entities;
using RentACarProject.Persistence.Context;

namespace RentACarProject.Persistence.Repositories
{
    public class EfCustomerRepository : EfGenericRepository<Customer>, ICustomerRepository
    {
        public EfCustomerRepository(RentACarDbContext context) : base(context)
        {
        }

        public async Task<Customer?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Email != null && c.Email.ToLower() == email.ToLower());
        }

        public async Task<Customer?> GetByPhoneAsync(string phone)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Phone != null && c.Phone == phone);
        }
    }
}
