using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Application.Exceptions;
using RentACarProject.Domain.Entities;
using RentACarProject.Domain.Enums;

namespace RentACarProject.Infrastructure.Services.Payments
{
    public class InternalCardPaymentService : IPaymentStrategyService<CreateCardPaymentDto>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public InternalCardPaymentService(
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

        public async Task<PaymentResponseDto> ProcessPaymentAsync(CreateCardPaymentDto dto)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(dto.ReservationId);
            if (reservation == null || reservation.IsDeleted)
                throw new NotFoundException("Rezervasyon bulunamadı.");

            var isValid = !string.IsNullOrWhiteSpace(dto.CardHolderName)
                       && !string.IsNullOrWhiteSpace(dto.CardNumber)
                       && dto.ExpireMonth > 0
                       && dto.ExpireYear > 0
                       && !string.IsNullOrWhiteSpace(dto.Cvc);

            var transactionId = $"CARD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString()[..6]}";

            var payment = new Payment
            {
                PaymentId = Guid.NewGuid(),
                ReservationId = dto.ReservationId,
                Amount = dto.Amount,
                PaymentDate = DateTime.UtcNow,
                Type = PaymentType.CreditCard,
                Status = isValid ? PaymentStatus.Completed : PaymentStatus.Failed,
                CardHolderName = dto.CardHolderName,
                CardNumberMasked = MaskCardNumber(dto.CardNumber),
                ExpireMonth = dto.ExpireMonth,
                ExpireYear = dto.ExpireYear,   
                InstallmentCount = dto.InstallmentCount,
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
                InstallmentCount = payment.InstallmentCount
            };
        }

        private string MaskCardNumber(string cardNumber)
        {
            if (cardNumber.Length < 4) return "****";
            var last4 = cardNumber[^4..];
            return $"**** **** **** {last4}";
        }
    }
}
