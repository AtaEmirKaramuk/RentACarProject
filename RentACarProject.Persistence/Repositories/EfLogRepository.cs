using RentACarProject.Domain.Entities;
using RentACarProject.Persistence.Context;

namespace RentACarProject.Persistence.Repositories
{
    public class EfLogRepository : ILogRepository
    {
        private readonly RentACarDbContext _context;

        public EfLogRepository(RentACarDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Log log)
        {
            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}
