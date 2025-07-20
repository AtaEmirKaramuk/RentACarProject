using RentACarProject.Domain.Common;
using RentACarProject.Domain.Enums;

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

        public CarStatus Status { get; set; } = CarStatus.Available;
        public VehicleClass VehicleClass { get; set; } = VehicleClass.Sedan;
        public FuelType FuelType { get; set; } = FuelType.Gasoline;
        public TransmissionType TransmissionType { get; set; } = TransmissionType.Automatic;

        public Model Model { get; set; } = null!;
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
