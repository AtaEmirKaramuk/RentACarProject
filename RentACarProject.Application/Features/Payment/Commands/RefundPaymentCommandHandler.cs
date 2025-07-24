using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.Exceptions;
using RentACarProject.Application.Services;
using RentACarProject.Domain.Enums;
using PaymentEntity = RentACarProject.Domain.Entities.Payment;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class RefundPaymentCommandHandler : IRequestHandler<RefundPaymentCommand, bool>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public RefundPaymentCommandHandler(
            IPaymentRepository paymentRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(request.PaymentId);
            if (payment == null || payment.IsDeleted)
                throw new NotFoundException("Ödeme bulunamadı.");

            if (payment.Reservation.Customer.UserId != _currentUserService.UserId)
                throw new ForbiddenAccessException("Bu ödeme kaydını iade etme yetkiniz yok.");

            if (payment.Status == PaymentStatus.Refunded)
                throw new BusinessException("Bu ödeme zaten iade edildi.");

            if (payment.Status != PaymentStatus.Completed && payment.Status != PaymentStatus.Pending)
                throw new BusinessException("Sadece tamamlanmış veya bekleyen ödemeler iade edilebilir.");

            payment.Status = PaymentStatus.Refunded;
            payment.ModifiedByUserId = _currentUserService.UserId;
            payment.ModifiedDate = DateTime.UtcNow;

            await _paymentRepository.UpdateAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
