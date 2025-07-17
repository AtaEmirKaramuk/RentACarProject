using RentACarProject.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentACarProject.Domain.Entities
{
    public class Customer : BaseEntity
    {
        [Key]
        public Guid UserId { get; set; } // Artık GUID
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public User User { get; set; } = null!;
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}