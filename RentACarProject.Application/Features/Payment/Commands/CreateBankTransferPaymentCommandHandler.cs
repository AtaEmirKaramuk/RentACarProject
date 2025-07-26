using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Application.Exceptions;
using RentACarProject.Domain.Enums;
using System.Text.RegularExpressions;
using PaymentEntity = RentACarProject.Domain.Entities.Payment;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class CreateBankTransferPaymentCommandHandler : IRequestHandler<CreateBankTransferPaymentCommand, PaymentResponseDto>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateBankTransferPaymentCommandHandler(
            IReservationRepository reservationRepository,
            IPaymentRepository paymentRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _reservationRepository = reservationRepository;
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PaymentResponseDto> Handle(CreateBankTransferPaymentCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Payment;

            var reservation = await _reservationRepository.GetReservationByIdAsync(dto.ReservationId);
            if (reservation == null || reservation.IsDeleted)
                throw new NotFoundException("Rezervasyon bulunamadı.");

            if (reservation.Customer.UserId != _currentUserService.UserId)
                throw new ForbiddenAccessException("Bu rezervasyon size ait değil.");

            if (reservation.Status != ReservationStatus.Pending)
                throw new BusinessException("Sadece bekleyen rezervasyonlar için ödeme yapılabilir.");

            if (dto.Amount <= 0)
                throw new BusinessException("Ödeme tutarı sıfırdan büyük olmalıdır.");

            var existingPayments = await _paymentRepository.GetPaymentsByReservationIdAsync(reservation.Id);
            if (existingPayments.Any(p => p.Status == PaymentStatus.Completed))
                throw new BusinessException("Bu rezervasyon için zaten başarılı bir ödeme yapılmış.");

            if (string.IsNullOrWhiteSpace(dto.SenderIban) || !Regex.IsMatch(dto.SenderIban, @"^TR\d{24}$"))
                throw new BusinessException("Geçerli bir IBAN girilmelidir. (TR ile başlamalı ve 26 karakter olmalı)");

            if (string.IsNullOrWhiteSpace(dto.SenderName))
                throw new BusinessException("Gönderen adı boş olamaz.");

            var transactionId = $"BANK-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..6]}";

            var payment = new PaymentEntity
            {
                ReservationId = dto.ReservationId,
                Amount = dto.Amount,
                PaymentDate = DateTime.UtcNow,
                Type = PaymentType.BankTransfer,
                Status = PaymentStatus.Pending,
                TransactionId = transactionId,
                SenderIban = dto.SenderIban,
                SenderName = dto.SenderName,
                CreatedByUserId = _currentUserService.UserId
            };

            await _paymentRepository.AddAsync(payment);
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
