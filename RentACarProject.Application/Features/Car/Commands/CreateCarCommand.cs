using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Car;
using RentACarProject.Domain.Enums;

namespace RentACarProject.Application.Features.Car.Commands
{
    public class CreateCarCommand : IRequest<ServiceResponse<CarResponseDto>>
    {
        public Guid ModelId { get; set; }
        public int Year { get; set; }
        public string Plate { get; set; } = null!;
        public decimal DailyPrice { get; set; }
        public string? Description { get; set; }

        public VehicleClass VehicleClass { get; set; }
        public FuelType FuelType { get; set; }
        public TransmissionType TransmissionType { get; set; }
    }
}
