using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Car;

namespace RentACarProject.Application.Features.Car.Queries
{
    public class GetAllCarsQuery : IRequest<ServiceResponse<List<CarResponseDto>>>
    {
    }
}
