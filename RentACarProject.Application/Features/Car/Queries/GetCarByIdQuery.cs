using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Car;

namespace RentACarProject.Application.Features.Car.Queries
{
    public class GetCarByIdQuery : IRequest<ServiceResponse<CarResponseDto>>
    {
        public Guid CarId { get; set; }
    }
}
