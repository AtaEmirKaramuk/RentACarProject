using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Reservation;

namespace RentACarProject.Application.Features.Reservation.Queries
{
    public class GetReservationsByCarIdQuery : IRequest<ServiceResponse<List<ReservationResponseDto>>>
    {
        public Guid CarId { get; set; }

        public GetReservationsByCarIdQuery() { }

        public GetReservationsByCarIdQuery(Guid carId)
        {
            CarId = carId;
        }
    }
}
