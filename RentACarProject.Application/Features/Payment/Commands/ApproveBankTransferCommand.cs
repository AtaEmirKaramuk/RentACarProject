using MediatR;
using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class ApproveBankTransferCommand : IRequest<PaymentResponseDto>
    {
        public Guid PaymentId { get; set; }
        public string TransactionId { get; set; }

        public ApproveBankTransferCommand(Guid paymentId, string transactionId)
        {
            if (paymentId == Guid.Empty)
                throw new ArgumentException("PaymentId boş olamaz.");

            if (string.IsNullOrWhiteSpace(transactionId))
                throw new ArgumentException("TransactionId boş olamaz.");

            PaymentId = paymentId;
            TransactionId = transactionId.Trim();
        }
    }
}
