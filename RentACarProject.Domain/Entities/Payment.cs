using RentACarProject.Domain.Common;
using RentACarProject.Domain.Enums;

namespace RentACarProject.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public Guid PaymentId { get; set; } = Guid.NewGuid();

        public Guid ReservationId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public PaymentType Type { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public string? TransactionId { get; set; } 

        
        public string? SenderIban { get; set; }
        public string? SenderName { get; set; }

        public Reservation Reservation { get; set; } = null!;
    }
}
