using MediatR;
using RentACarProject.Application.Common;
using System;

namespace RentACarProject.Application.Features.Reservation.Commands
{
    public class CancelReservationCommand : IRequest<ServiceResponse<string>>
    {
        public Guid Id { get; set; }
    }
}
