using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Domain.DTOs.Car;

namespace RentACarProject.Application.Features.Car.Queries
{
    public class GetCarsByModelQuery : IRequest<ServiceResponse<List<CarResponseDto>>>
    {
        public string ModelName { get; set; } = null!;
    }
}
