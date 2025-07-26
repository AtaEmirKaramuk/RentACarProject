using MediatR;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class CancelPaymentCommand : IRequest<bool>
    {
        public Guid PaymentId { get; }

        public CancelPaymentCommand(Guid paymentId)
        {
            PaymentId = paymentId != Guid.Empty
                ? paymentId
                : throw new ArgumentException("PaymentId boş olamaz.");
        }
    }
}
