using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Car;

namespace RentACarProject.Application.Features.Car.Queries
{
    public class GetCarsByBrandQuery : IRequest<ServiceResponse<List<CarResponseDto>>>
    {
        public string BrandName { get; set; } = null!;
    }
}
