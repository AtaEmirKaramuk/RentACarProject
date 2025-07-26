using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Domain.Entities;
using RentACarProject.Domain.Enums;
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
                .Where(p => p.ReservationId == reservationId && !p.IsDeleted)
                .Include(p => p.Reservation)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentsByUserIdAsync(Guid userId)
        {
            return await _context.Payments
                .Include(p => p.Reservation)
                .ThenInclude(r => r.Customer)
                .Where(p => p.Reservation.Customer.UserId == userId && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<Payment?> GetPaymentByIdAsync(Guid paymentId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.Id == paymentId && !p.IsDeleted);
        }

        public async Task<List<Payment>> GetUserPaymentsWithFiltersAsync(Guid userId, DateTime? startDate = null, DateTime? endDate = null, PaymentStatus? status = null, PaymentType? type = null)
        {
            var query = _context.Payments
                .Include(p => p.Reservation)
                .ThenInclude(r => r.Customer)
                .Where(p => p.Reservation.Customer.UserId == userId && !p.IsDeleted)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(p => p.PaymentDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(p => p.PaymentDate <= endDate.Value);

            if (status.HasValue)
                query = query.Where(p => p.Status == status.Value);

            if (type.HasValue)
                query = query.Where(p => p.Type == type.Value);

            return await query.ToListAsync();
        }

        public async Task<List<Payment>> GetAllPaymentsWithFiltersAsync(DateTime? startDate = null, DateTime? endDate = null, PaymentStatus? status = null, PaymentType? type = null, Guid? reservationId = null, Guid? userId = null)
        {
            var query = _context.Payments
                .Include(p => p.Reservation)
                .ThenInclude(r => r.Customer)
                .Where(p => !p.IsDeleted)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(p => p.PaymentDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(p => p.PaymentDate <= endDate.Value);

            if (status.HasValue)
                query = query.Where(p => p.Status == status.Value);

            if (type.HasValue)
                query = query.Where(p => p.Type == type.Value);

            if (reservationId.HasValue)
                query = query.Where(p => p.ReservationId == reservationId.Value);

            if (userId.HasValue)
                query = query.Where(p => p.Reservation.Customer.UserId == userId.Value);

            return await query.ToListAsync();
        }

        public async Task<List<Payment>> GetPendingBankTransferPaymentsAsync()
        {
            return await _context.Payments
                .Include(p => p.Reservation)
                .ThenInclude(r => r.Customer)
                .Where(p => p.Type == PaymentType.BankTransfer && p.Status == PaymentStatus.Pending && !p.IsDeleted)
                .ToListAsync();
        }
    }
}
