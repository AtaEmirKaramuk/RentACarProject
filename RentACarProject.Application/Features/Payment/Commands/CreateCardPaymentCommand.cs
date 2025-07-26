using MediatR;
using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class CreateCardPaymentCommand : IRequest<PaymentResponseDto>
    {
        public CreateCardPaymentDto Payment { get; set; }

        public CreateCardPaymentCommand(CreateCardPaymentDto payment)
        {
            Payment = payment ?? throw new ArgumentNullException(nameof(payment));
        }
    }
}
