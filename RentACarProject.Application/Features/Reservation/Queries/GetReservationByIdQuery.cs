using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Reservation;

namespace RentACarProject.Application.Features.Reservation.Queries
{
    public class GetReservationByIdQuery : IRequest<ServiceResponse<ReservationResponseDto>>
    {
        public Guid Id { get; set; }

        public GetReservationByIdQuery() { }

        public GetReservationByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
