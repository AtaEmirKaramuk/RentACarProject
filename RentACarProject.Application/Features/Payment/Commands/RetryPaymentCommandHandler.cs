using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Application.Exceptions;
using RentACarProject.Application.Services;
using RentACarProject.Domain.Entities;
using RentACarProject.Domain.Enums;
using PaymentEntity = RentACarProject.Domain.Entities.Payment;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class RetryPaymentCommandHandler : IRequestHandler<RetryPaymentCommand, PaymentResponseDto>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public RetryPaymentCommandHandler(
            IPaymentRepository paymentRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PaymentResponseDto> Handle(RetryPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(request.PaymentId);
            if (payment == null || payment.IsDeleted)
                throw new NotFoundException("Ödeme bulunamadı.");

            if (payment.Reservation.Customer.UserId != _currentUserService.UserId)
                throw new ForbiddenAccessException("Bu ödeme kaydını tekrar deneme yetkiniz yok.");

            if (payment.Type != PaymentType.CreditCard)
                throw new BusinessException("Sadece kredi kartı ödemeleri tekrar denenebilir.");

            if (payment.Status != PaymentStatus.Failed)
                throw new BusinessException("Yalnızca başarısız ödemeler tekrar denenebilir.");

            payment.TransactionId = Guid.NewGuid().ToString();
            payment.Status = PaymentStatus.Completed;
            payment.ModifiedByUserId = _currentUserService.UserId;
            payment.ModifiedDate = DateTime.UtcNow;

            await _paymentRepository.UpdateAsync(payment);
            await _unitOfWork.SaveChangesAsync();

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
