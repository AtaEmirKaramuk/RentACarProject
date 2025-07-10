using RentACarProject.Domain.Common;

namespace RentACarProject.Domain.Entities
{
    public class Car : BaseEntity
    {
        public Guid CarId { get; set; } = Guid.NewGuid();
        public Guid ModelId { get; set; }
        public int Year { get; set; }
        public string Plate { get; set; } = null!;
        public decimal DailyPrice { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; } = true;

        public Model Model { get; set; } = null!;
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
