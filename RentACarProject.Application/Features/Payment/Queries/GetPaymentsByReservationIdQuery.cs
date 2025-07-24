using MediatR;
using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Features.Payment.Queries
{
    public class GetPaymentsByReservationIdQuery : IRequest<List<PaymentResponseDto>>
    {
        public Guid ReservationId { get; set; }

        public GetPaymentsByReservationIdQuery(Guid reservationId)
        {
            ReservationId = reservationId;
        }
    }
}
