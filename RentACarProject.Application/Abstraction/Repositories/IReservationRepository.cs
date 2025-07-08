using RentACarProject.Domain.Entities;

namespace RentACarProject.Application.Abstraction.Repositories
{
    public interface IReservationRepository : IGenericRepository<Reservation>
    {
        Task<List<Reservation>> GetActiveReservationsAsync();
        Task<List<Reservation>> GetReservationsByUserIdAsync(Guid userId);
        Task<List<Reservation>> GetReservationsByCarIdAsync(Guid carId);
        //opsiyonel
        Task<List<Reservation>> GetCompletedReservationsAsync();
        Task<List<Reservation>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Reservation?> GetReservationWithPaymentsAsync(Guid reservationId);
    }
}
