using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Reservation;

namespace RentACarProject.Application.Features.Reservation.Commands
{
    public class CreateReservationCommand : IRequest<ServiceResponse<ReservationResponseDto>>
    {
        public CreateReservationDto Reservation { get; set; } = null!;
    }
}
