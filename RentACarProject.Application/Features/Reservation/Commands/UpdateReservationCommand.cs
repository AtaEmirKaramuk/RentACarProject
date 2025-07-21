using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Reservation;

namespace RentACarProject.Application.Features.Reservation.Commands
{
    public class UpdateReservationCommand : IRequest<ServiceResponse<ReservationResponseDto>>
    {
        public UpdateReservationDto Reservation { get; set; } = null!;
    }
}
