using MediatR;
using RentACarProject.Domain.Enums;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class ChangePaymentStatusCommand : IRequest<bool>
    {
        public Guid PaymentId { get; }
        public PaymentStatus NewStatus { get; }

        public ChangePaymentStatusCommand(Guid paymentId, PaymentStatus newStatus)
        {
            if (paymentId == Guid.Empty)
                throw new ArgumentException("PaymentId boş olamaz.");

            PaymentId = paymentId;
            NewStatus = newStatus;
        }
    }
}
