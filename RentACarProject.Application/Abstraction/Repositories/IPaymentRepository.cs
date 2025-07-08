using RentACarProject.Domain.Entities;

namespace RentACarProject.Application.Abstraction.Repositories
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<List<Payment>> GetPaymentsByReservationIdAsync(Guid reservationId);
        Task<List<Payment>> GetPaymentsByUserIdAsync(Guid userId);
    }
}
