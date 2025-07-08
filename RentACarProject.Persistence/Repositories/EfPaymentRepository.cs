using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Domain.Entities;
using RentACarProject.Persistence.Context;

namespace RentACarProject.Persistence.Repositories
{
    public class EfPaymentRepository : EfGenericRepository<Payment>, IPaymentRepository
    {
        public EfPaymentRepository(RentACarDbContext context) : base(context)
        {
        }

        public async Task<List<Payment>> GetPaymentsByReservationIdAsync(Guid reservationId)
        {
            return await _context.Payments
                .Where(p => p.ReservationId == reservationId)
                .Include(p => p.Reservation)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentsByUserIdAsync(Guid userId)
        {
            return await _context.Payments
                .Include(p => p.Reservation)
                .ThenInclude(r => r.Customer)
                .Where(p => p.Reservation.Customer.UserId == userId)
                .ToListAsync();
        }
    }
}
