using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Reservation;

namespace RentACarProject.Application.Features.Reservation.Queries
{
    public class GetReservationsByUserIdQuery : IRequest<ServiceResponse<List<ReservationResponseDto>>>
    {
        public Guid UserId { get; set; }

        public GetReservationsByUserIdQuery() { }

        public GetReservationsByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
