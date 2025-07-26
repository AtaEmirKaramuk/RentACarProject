using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Application.Exceptions;
using RentACarProject.Domain.Enums;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class ApproveBankTransferCommandHandler : IRequestHandler<ApproveBankTransferCommand, PaymentResponseDto>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ApproveBankTransferCommandHandler(
            IPaymentRepository paymentRepository,
            IReservationRepository reservationRepository,
            IUnitOfWork unitOfWork)
        {
            _paymentRepository = paymentRepository;
            _reservationRepository = reservationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaymentResponseDto> Handle(ApproveBankTransferCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(request.PaymentId);
            if (payment == null || payment.IsDeleted)
                throw new NotFoundException("Ödeme bulunamadı.");

            if (payment.Type != PaymentType.BankTransfer)
                throw new BusinessException("Sadece banka havalesi ödemeleri onaylanabilir.");

            if (payment.Status == PaymentStatus.Completed)
                throw new BusinessException("Bu ödeme zaten onaylanmış.");

            if (string.IsNullOrWhiteSpace(request.TransactionId) || request.TransactionId.Length > 100)
                throw new BusinessException("Geçerli bir işlem numarası giriniz.");

            var reservation = await _reservationRepository.GetReservationByIdAsync(payment.ReservationId);
            if (reservation == null || reservation.IsDeleted)
                throw new NotFoundException("İlgili rezervasyon bulunamadı.");

            if (reservation.Status == ReservationStatus.Completed)
                throw new BusinessException("Bu rezervasyon zaten tamamlanmış.");

            var existingPayments = await _paymentRepository.GetPaymentsByReservationIdAsync(payment.ReservationId);
            if (existingPayments.Any(p => p.Status == PaymentStatus.Completed))
                throw new BusinessException("Bu rezervasyon için zaten başarılı bir ödeme yapılmış.");

            if (string.IsNullOrWhiteSpace(payment.SenderIban) || !payment.SenderIban.StartsWith("TR"))
                throw new BusinessException("Geçersiz IBAN.");

            if (string.IsNullOrWhiteSpace(payment.SenderName))
                throw new BusinessException("Gönderen adı eksik.");

            payment.Status = PaymentStatus.Completed;
            payment.TransactionId = request.TransactionId.Trim();
            payment.ModifiedDate = DateTime.UtcNow;
            await _paymentRepository.UpdateAsync(payment);

            reservation.Status = ReservationStatus.Completed;
            reservation.ModifiedDate = DateTime.UtcNow;
            await _reservationRepository.UpdateAsync(reservation);

            await _unitOfWork.SaveChangesAsync();

            return new PaymentResponseDto
            {
                Id = payment.Id,
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
