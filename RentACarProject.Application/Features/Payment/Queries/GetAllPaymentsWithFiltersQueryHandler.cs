using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Features.Payment.Queries
{
    public class GetAllPaymentsWithFiltersQueryHandler : IRequestHandler<GetAllPaymentsWithFiltersQuery, ServiceResponse<List<PaymentResponseDto>>>
    {
        private readonly IPaymentRepository _paymentRepository;

        public GetAllPaymentsWithFiltersQueryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<ServiceResponse<List<PaymentResponseDto>>> Handle(GetAllPaymentsWithFiltersQuery request, CancellationToken cancellationToken)
        {
            var payments = await _paymentRepository.GetAllPaymentsWithFiltersAsync(
                request.StartDate,
                request.EndDate,
                request.Status,
                request.Type,
                request.ReservationId,
                request.UserId
            );

            var result = payments.Select(p => new PaymentResponseDto
            {
                Id = p.Id,
                ReservationId = p.ReservationId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                Type = p.Type,
                Status = p.Status,
                TransactionId = p.TransactionId,
                SenderIban = p.SenderIban,
                SenderName = p.SenderName
            }).ToList();

            return new ServiceResponse<List<PaymentResponseDto>>
            {
                Success = true,
                Message = "Filtrelenmiş ödemeler listelendi.",
                Data = result
            };
        }
    }
}
