using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Reservation;

namespace RentACarProject.Application.Features.Reservation.Queries
{
    public class GetReservationWithPaymentsQuery : IRequest<ServiceResponse<ReservationWithPaymentsDto>>
    {
        public Guid ReservationId { get; set; }

        public GetReservationWithPaymentsQuery() { }

        public GetReservationWithPaymentsQuery(Guid reservationId)
        {
            ReservationId = reservationId;
        }
    }
}
