using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Domain.DTOs.Car;

namespace RentACarProject.Application.Features.Car.Commands
{
    public class DeleteCarCommand : IRequest<ServiceResponse<DeletedCarDto>>
    {
        public Guid Id { get; set; }
    }
}
