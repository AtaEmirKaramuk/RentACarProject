using MediatR;
using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class CreateBankTransferPaymentCommand : IRequest<PaymentResponseDto>
    {
        public CreateBankTransferDto Payment { get; }

        public CreateBankTransferPaymentCommand(CreateBankTransferDto payment)
        {
            Payment = payment;
        }
    }
}
