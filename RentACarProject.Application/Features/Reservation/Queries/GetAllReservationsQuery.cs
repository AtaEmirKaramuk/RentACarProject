using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Reservation;

namespace RentACarProject.Application.Features.Reservation.Queries
{
    public class GetAllReservationsQuery : IRequest<ServiceResponse<List<ReservationResponseDto>>>
    {
    }
}
