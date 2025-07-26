using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.Exceptions;
using RentACarProject.Domain.Enums;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class CancelPaymentCommandHandler : IRequestHandler<CancelPaymentCommand, bool>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CancelPaymentCommandHandler(IPaymentRepository paymentRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(CancelPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(request.PaymentId);
            if (payment == null || payment.IsDeleted)
                throw new NotFoundException("Ödeme bulunamadı.");

            if (payment.Reservation.Customer.UserId != _currentUserService.UserId)
                throw new ForbiddenAccessException("Bu ödeme kaydına erişim yetkiniz yok.");

            if (payment.Status != PaymentStatus.Pending && payment.Status != PaymentStatus.Failed)
                throw new BusinessException("Sadece bekleyen veya başarısız ödemeler iptal edilebilir.");

            payment.Status = PaymentStatus.Cancelled;
            payment.ModifiedByUserId = _currentUserService.UserId;
            payment.ModifiedDate = DateTime.UtcNow;

            await _paymentRepository.UpdateAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
