using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Car;
using RentACarProject.Domain.Enums;

namespace RentACarProject.Application.Features.Car.Queries
{
    public class GetAllCarsQuery : IRequest<ServiceResponse<List<CarResponseDto>>>
    {
        public Guid? BrandId { get; set; }
        public Guid? ModelId { get; set; }
        public string? BrandName { get; set; }
        public string? ModelName { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public CarStatus? Status { get; set; }

        
        public VehicleClass? VehicleClass { get; set; }
        public FuelType? FuelType { get; set; }
        public TransmissionType? TransmissionType { get; set; }
    }
}
