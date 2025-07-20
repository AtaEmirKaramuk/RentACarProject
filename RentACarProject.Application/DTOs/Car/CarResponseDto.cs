using RentACarProject.Domain.Enums;

namespace RentACarProject.Application.DTOs.Car
{
    public class CarResponseDto
    {
        public Guid Id { get; set; }
        public string BrandName { get; set; } = null!;
        public string ModelName { get; set; } = null!;
        public int Year { get; set; }
        public string Plate { get; set; } = null!;
        public decimal DailyPrice { get; set; }
        public string? Description { get; set; }

        public CarStatus Status { get; set; }
        public VehicleClass VehicleClass { get; set; }
        public FuelType FuelType { get; set; }
        public TransmissionType TransmissionType { get; set; }
    }
}
