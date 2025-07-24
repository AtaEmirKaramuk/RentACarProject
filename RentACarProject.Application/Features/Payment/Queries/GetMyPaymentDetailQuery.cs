using MediatR;
using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Features.Payment.Queries
{
    public class GetMyPaymentDetailQuery : IRequest<PaymentResponseDto>
    {
        public Guid PaymentId { get; set; }

        public GetMyPaymentDetailQuery(Guid paymentId)
        {
            PaymentId = paymentId;
        }
    }
}
