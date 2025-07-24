using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Application.Services;

namespace RentACarProject.Application.Features.Payment.Queries
{
    public class GetMyPaymentsQueryHandler : IRequestHandler<GetMyPaymentsQuery, List<PaymentResponseDto>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetMyPaymentsQueryHandler(IPaymentRepository paymentRepository, ICurrentUserService currentUserService)
        {
            _paymentRepository = paymentRepository;
            _currentUserService = currentUserService;
        }

        public async Task<List<PaymentResponseDto>> Handle(GetMyPaymentsQuery request, CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId == null)
                throw new UnauthorizedAccessException("Kullanıcı oturum bilgisi bulunamadı.");

            var userId = _currentUserService.UserId.Value;

            var payments = await _paymentRepository.GetUserPaymentsWithFiltersAsync(
                userId,
                request.StartDate,
                request.EndDate,
                request.Status,
                request.Type
            );

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
