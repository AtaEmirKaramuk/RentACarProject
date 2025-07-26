using RentACarProject.Domain.Common;

namespace RentACarProject.Domain.Entities
{
    public class Location : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Description { get; set; }

        public ICollection<Reservation> PickupReservations { get; set; } = new List<Reservation>();
        public ICollection<Reservation> DropoffReservations { get; set; } = new List<Reservation>();
    }
}
