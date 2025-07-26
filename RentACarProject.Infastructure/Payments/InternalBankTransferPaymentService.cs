using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Application.Exceptions;
using RentACarProject.Domain.Entities;
using RentACarProject.Domain.Enums;

namespace RentACarProject.Infrastructure.Services.Payments
{
    public class InternalBankTransferPaymentService : IPaymentStrategyService<CreateBankTransferDto>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public InternalBankTransferPaymentService(
            IPaymentRepository paymentRepository,
            IReservationRepository reservationRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _paymentRepository = paymentRepository;
            _reservationRepository = reservationRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PaymentResponseDto> ProcessPaymentAsync(CreateBankTransferDto dto)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(dto.ReservationId);
            if (reservation == null || reservation.IsDeleted)
                throw new NotFoundException("Rezervasyon bulunamadı.");

            var transactionId = $"BANK-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString()[..6]}";

            var payment = new Payment
            {
                PaymentId = Guid.NewGuid(),
                ReservationId = dto.ReservationId,
                Amount = dto.Amount,
                PaymentDate = DateTime.UtcNow,
                Type = PaymentType.BankTransfer,
                Status = PaymentStatus.Pending,
                SenderIban = dto.SenderIban,
                SenderName = dto.SenderName,
                TransactionId = transactionId,
                CreatedByUserId = _currentUserService.UserId
            };

            await _paymentRepository.AddAsync(payment);
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
