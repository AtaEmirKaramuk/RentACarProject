using MediatR;
using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class CreatePaymentCommand : IRequest<PaymentResponseDto>
    {
        public CreatePaymentDto Payment { get; set; }

        public CreatePaymentCommand(CreatePaymentDto payment)
        {
            Payment = payment;
        }
    }
}
