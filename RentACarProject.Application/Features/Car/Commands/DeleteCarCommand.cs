using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Car;

namespace RentACarProject.Application.Features.Car.Commands
{
    public class DeleteCarCommand : IRequest<ServiceResponse<DeletedCarDto>>
    {
        public Guid CarId { get; set; }
    }
}
