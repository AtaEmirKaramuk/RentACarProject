using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Features.Payment.Queries
{
    public class GetPendingBankTransferPaymentsQueryHandler : IRequestHandler<GetPendingBankTransferPaymentsQuery, List<PaymentResponseDto>>
    {
        private readonly IPaymentRepository _paymentRepository;

        public GetPendingBankTransferPaymentsQueryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<List<PaymentResponseDto>> Handle(GetPendingBankTransferPaymentsQuery request, CancellationToken cancellationToken)
        {
            var payments = await _paymentRepository.GetPendingBankTransferPaymentsAsync();

            return payments.Select(p => new PaymentResponseDto
            {
                PaymentId = p.PaymentId,
                ReservationId = p.ReservationId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                Type = p.Type,
                Status = p.Status,
                TransactionId = p.TransactionId,
                SenderIban = p.SenderIban,
                SenderName = p.SenderName
            }).ToList();
        }
    }
}
