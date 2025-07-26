using RentACarProject.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentACarProject.Domain.Entities
{
    public class Customer : BaseEntity
    {
        [Key] 
        [ForeignKey("User")] 
        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
