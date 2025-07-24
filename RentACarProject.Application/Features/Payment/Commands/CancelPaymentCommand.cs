using MediatR;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class CancelPaymentCommand : IRequest<bool>
    {
        public Guid PaymentId { get; set; }

        public CancelPaymentCommand(Guid paymentId)
        {
            PaymentId = paymentId;
        }
    }
}
