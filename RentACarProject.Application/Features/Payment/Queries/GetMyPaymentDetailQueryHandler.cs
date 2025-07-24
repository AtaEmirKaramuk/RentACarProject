using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Application.Exceptions;
using RentACarProject.Application.Services;

namespace RentACarProject.Application.Features.Payment.Queries
{
    public class GetMyPaymentDetailQueryHandler : IRequestHandler<GetMyPaymentDetailQuery, PaymentResponseDto>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetMyPaymentDetailQueryHandler(IPaymentRepository paymentRepository, ICurrentUserService currentUserService)
        {
            _paymentRepository = paymentRepository;
            _currentUserService = currentUserService;
        }

        public async Task<PaymentResponseDto> Handle(GetMyPaymentDetailQuery request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(request.PaymentId);
            if (payment == null || payment.IsDeleted)
                throw new NotFoundException("Ödeme bulunamadı.");

            if (payment.Reservation.Customer.UserId != _currentUserService.UserId)
                throw new ForbiddenAccessException("Bu ödeme kaydına erişim yetkiniz yok.");

            return new PaymentResponseDto
            {
                PaymentId = payment.PaymentId,
                ReservationId = payment.ReservationId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                Type = payment.Type,
                Status = payment.Status,
                TransactionId = payment.TransactionId,
                SenderIban = payment.SenderIban,
                SenderName = payment.SenderName
            };
        }
    }
}
