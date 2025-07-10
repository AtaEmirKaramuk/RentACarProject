using MediatR;
using RentACarProject.Application.Common;

namespace RentACarProject.Application.Features.Car.Commands
{
    public class DeleteCarCommand : IRequest<ServiceResponse<Guid>>
    {
        public Guid CarId { get; set; }
    }
}
