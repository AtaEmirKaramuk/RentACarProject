using RentACarProject.Domain.Common;
using System;
using System.Collections.Generic;

namespace RentACarProject.Domain.Entities
{
    public class Reservation : BaseEntity
    {
        public Guid ReservationId { get; set; } = Guid.NewGuid();
        public Guid CarId { get; set; }
        public Guid CustomerId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }

        public Car Car { get; set; } = null!;
        public Customer Customer { get; set; } = null!;
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
