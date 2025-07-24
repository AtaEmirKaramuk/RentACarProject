using MediatR;
using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class UpdatePaymentCommand : IRequest<PaymentResponseDto>
    {
        public UpdatePaymentDto Payment { get; set; }

        public UpdatePaymentCommand(UpdatePaymentDto payment)
        {
            Payment = payment;
        }
    }
}
