using RentACarProject.Domain.Common;
using System;

namespace RentACarProject.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public Guid PaymentId { get; set; } = Guid.NewGuid();
        public Guid ReservationId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? PaymentType { get; set; }

        public Reservation Reservation { get; set; } = null!;
    }
}
