using MediatR;
using RentACarProject.Domain.Enums;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class ChangePaymentStatusCommand : IRequest<bool>
    {
        public Guid PaymentId { get; set; }
        public PaymentStatus NewStatus { get; set; }

        public ChangePaymentStatusCommand(Guid paymentId, PaymentStatus newStatus)
        {
            PaymentId = paymentId;
            NewStatus = newStatus;
        }
    }
}
