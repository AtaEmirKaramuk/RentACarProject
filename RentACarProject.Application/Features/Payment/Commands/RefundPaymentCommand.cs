using MediatR;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class RefundPaymentCommand : IRequest<bool>
    {
        public Guid PaymentId { get; set; }
        public string? Reason { get; set; }

        public RefundPaymentCommand(Guid paymentId, string? reason = null)
        {
            PaymentId = paymentId;
            Reason = reason;
        }
    }
}
