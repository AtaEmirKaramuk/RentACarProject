using RentACarProject.Domain.Entities;
using RentACarProject.Domain.Enums;

namespace RentACarProject.Application.Abstraction.Repositories
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<List<Payment>> GetPaymentsByReservationIdAsync(Guid reservationId);
        Task<List<Payment>> GetPaymentsByUserIdAsync(Guid userId);
        Task<Payment?> GetPaymentByIdAsync(Guid paymentId);

       

        
        Task<List<Payment>> GetUserPaymentsWithFiltersAsync(
            Guid userId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            PaymentStatus? status = null,
            PaymentType? type = null);

        // Tüm ödemeleri filtreli şekilde getir (admin için)
        Task<List<Payment>> GetAllPaymentsWithFiltersAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            PaymentStatus? status = null,
            PaymentType? type = null,
            Guid? reservationId = null,
            Guid? userId = null);

        // Onay bekleyen havale ödemeleri (admin onayı için)
        Task<List<Payment>> GetPendingBankTransferPaymentsAsync();
    }
}
