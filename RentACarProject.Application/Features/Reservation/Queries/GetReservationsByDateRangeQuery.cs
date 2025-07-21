using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Reservation;

namespace RentACarProject.Application.Features.Reservation.Queries
{
    public class GetReservationsByDateRangeQuery : IRequest<ServiceResponse<List<ReservationResponseDto>>>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public GetReservationsByDateRangeQuery() { }

        public GetReservationsByDateRangeQuery(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
