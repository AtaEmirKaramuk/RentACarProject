using RentACarProject.Domain.Common;
using RentACarProject.Domain.Enums;
using System;
using System.Collections.Generic;

namespace RentACarProject.Domain.Entities
{
    public class Reservation : BaseEntity
    {
        public Guid ReservationId { get; set; } = Guid.NewGuid();
        public Guid CarId { get; set; }
        public Guid UserId { get; set; }
        public Guid PickupLocationId { get; set; }
        public Guid DropoffLocationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        ReservationStatus Status { get; set; } = ReservationStatus.Active;

        public Car Car { get; set; } = null!;
        public Customer Customer { get; set; } = null!;
        public Location PickupLocation { get; set; } = null!;
        public Location DropoffLocation { get; set; } = null!;
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}