using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Domain.Entities;
using RentACarProject.Persistence.Context;

namespace RentACarProject.Persistence.Repositories
{
    public class EfReservationRepository : EfGenericRepository<Reservation>, IReservationRepository
    {
        public EfReservationRepository(RentACarDbContext context) : base(context)
        {
        }

        public async Task<List<Reservation>> GetActiveReservationsAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Reservations
                .Where(r => r.StartDate <= now && r.EndDate >= now)
                .Include(r => r.Car)
                .Include(r => r.Customer)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetReservationsByUserIdAsync(Guid userId)
        {
            return await _context.Reservations
                .Include(r => r.Customer)
                .ThenInclude(c => c.User)
                .Where(r => r.Customer.UserId == userId)
                .Include(r => r.Car)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetReservationsByCarIdAsync(Guid carId)
        {
            return await _context.Reservations
                .Where(r => r.CarId == carId)
                .Include(r => r.Customer)
                .Include(r => r.Car)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetCompletedReservationsAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Reservations
                .Where(r => r.EndDate < now)
                .Include(r => r.Car)
                .Include(r => r.Customer)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Reservations
                .Where(r => r.StartDate >= startDate && r.EndDate <= endDate)
                .Include(r => r.Car)
                .Include(r => r.Customer)
                .ToListAsync();
        }

        public async Task<Reservation?> GetReservationWithPaymentsAsync(Guid reservationId)
        {
            return await _context.Reservations
                .Include(r => r.Payments)
                .Include(r => r.Car)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId);
        }
    }
}
