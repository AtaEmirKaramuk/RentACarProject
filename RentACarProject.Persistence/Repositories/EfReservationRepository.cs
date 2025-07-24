using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Domain.Entities;
using RentACarProject.Domain.Enums;
using RentACarProject.Persistence.Context;

namespace RentACarProject.Persistence.Repositories
{
    public class EfReservationRepository : EfGenericRepository<Reservation>, IReservationRepository
    {
        public EfReservationRepository(RentACarDbContext context) : base(context)
        {
        }

        // Aktif (şu an devam eden) rezervasyonları getirir.
        public async Task<List<Reservation>> GetActiveReservationsAsync()
        {
            var now = DateTime.UtcNow;

            return await _context.Reservations
                .Where(r => r.StartDate <= now && r.EndDate >= now && r.Status == ReservationStatus.Active)
                .Include(r => r.Car)
                .ToListAsync();
        }

        // Belirli bir kullanıcıya (UserId) ait rezervasyonları ilişkili verilerle birlikte getirir.
        public async Task<List<Reservation>> GetReservationsByUserIdAsync(Guid userId)
        {
            return await _context.Reservations
                .Where(r => r.UserId == userId)
                .Include(r => r.Car).ThenInclude(c => c.Model).ThenInclude(m => m.Brand)
                .Include(r => r.PickupLocation)
                .Include(r => r.DropoffLocation)
                .ToListAsync();
        }

        // Belirli bir araca ait rezervasyonları getirir.
        public async Task<List<Reservation>> GetReservationsByCarIdAsync(Guid carId)
        {
            return await _context.Reservations
                .Where(r => r.CarId == carId)
                .ToListAsync();
        }

        // Tamamlanmış (EndDate geçmiş ve status = Completed olan) rezervasyonları getirir.
        public async Task<List<Reservation>> GetCompletedReservationsAsync()
        {
            var now = DateTime.UtcNow;

            return await _context.Reservations
                .Where(r => r.EndDate < now && r.Status == ReservationStatus.Completed)
                .Include(r => r.Car)
                .ToListAsync();
        }

        // Verilen tarih aralığına denk gelen rezervasyonları getirir (çakışanlar).
        public async Task<List<Reservation>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Reservations
                .Where(r =>
                    r.StartDate < endDate &&
                    r.EndDate > startDate &&
                    r.Status == ReservationStatus.Active)
                .ToListAsync();
        }

        // Bir rezervasyonu ödeme bilgileri, araç, model, marka ve lokasyonlarla birlikte getirir.
        public async Task<Reservation?> GetReservationWithPaymentsAsync(Guid reservationId)
        {
            return await _context.Reservations
                .Include(r => r.Payments)
                .Include(r => r.Car).ThenInclude(c => c.Model).ThenInclude(m => m.Brand)
                .Include(r => r.PickupLocation)
                .Include(r => r.DropoffLocation)
                .FirstOrDefaultAsync(r => r.Id == reservationId);
        }


        public async Task<Reservation?> GetReservationByIdAsync(Guid id)
        {
            return await Query().FirstOrDefaultAsync(r => r.Id == id);
        }

        // Bir rezervasyonu tüm ilişkili verileriyle (Car, Model, Brand, Locations) birlikte getirir.
        public async Task<Reservation?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Reservations
                .Include(r => r.Car).ThenInclude(c => c.Model).ThenInclude(m => m.Brand)
                .Include(r => r.PickupLocation)
                .Include(r => r.DropoffLocation)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
