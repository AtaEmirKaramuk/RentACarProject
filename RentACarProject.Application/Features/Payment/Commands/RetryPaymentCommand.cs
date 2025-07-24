using MediatR;
using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class RetryPaymentCommand : IRequest<PaymentResponseDto>
    {
        public Guid PaymentId { get; set; }

        public RetryPaymentCommand(Guid paymentId)
        {
            PaymentId = paymentId;
        }
    }
}
