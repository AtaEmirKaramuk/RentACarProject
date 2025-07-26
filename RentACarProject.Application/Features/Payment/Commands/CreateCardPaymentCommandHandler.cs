using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Application.Exceptions;
using RentACarProject.Domain.Enums;
using System.Text.RegularExpressions;
using PaymentEntity = RentACarProject.Domain.Entities.Payment;
using ReservationEntity = RentACarProject.Domain.Entities.Reservation;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class CreateCardPaymentCommandHandler : IRequestHandler<CreateCardPaymentCommand, PaymentResponseDto>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCardPaymentCommandHandler(
            IReservationRepository reservationRepository,
            ICurrentUserService currentUserService,
            IPaymentRepository paymentRepository,
            IUnitOfWork unitOfWork)
        {
            _reservationRepository = reservationRepository;
            _currentUserService = currentUserService;
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaymentResponseDto> Handle(CreateCardPaymentCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Payment;

            // 1. Rezervasyon var mı ve geçerli mi
            var reservation = await _reservationRepository.GetReservationByIdAsync(dto.ReservationId);
            if (reservation == null || reservation.IsDeleted)
                throw new NotFoundException("Rezervasyon bulunamadı.");

            // 2. Kullanıcıya ait mi
            if (reservation.Customer.UserId != _currentUserService.UserId)
                throw new ForbiddenAccessException("Bu rezervasyon size ait değil.");

            // 3. Rezervasyon durumu uygun mu
            if (reservation.Status != ReservationStatus.Pending)
                throw new BusinessException("Sadece bekleyen rezervasyonlar için ödeme yapılabilir.");

            // 4. Tutar geçerli mi
            if (dto.Amount <= 0)
                throw new BusinessException("Ödeme tutarı sıfırdan büyük olmalıdır.");

            if (dto.Amount < reservation.TotalPrice)
                throw new BusinessException($"Rezervasyon tutarı {reservation.TotalPrice} TL'dir. Eksik ödeme yapılamaz.");

            // 5. Daha önce ödeme yapıldı mı
            var existingPayments = await _paymentRepository.GetPaymentsByReservationIdAsync(reservation.Id);
            if (existingPayments.Any(p => p.Status == PaymentStatus.Completed))
                throw new BusinessException("Bu rezervasyon için zaten başarılı bir ödeme yapılmış.");

            // 6. Kart bilgileri doğru mu
            if (!Regex.IsMatch(dto.CardNumber, @"^\d{16}$"))
                throw new BusinessException("Kart numarası 16 haneli olmalıdır.");

            var expiryDate = new DateTime(dto.ExpireYear, dto.ExpireMonth, 1).AddMonths(1).AddDays(-1);
            if (expiryDate < DateTime.UtcNow)
                throw new BusinessException("Kartın son kullanma tarihi geçmiş.");

            // 7. Taksit sayısı kontrolü
            if (dto.InstallmentCount.HasValue && (dto.InstallmentCount < 1 || dto.InstallmentCount > 12))
                throw new BusinessException("Taksit sayısı 1 ile 12 arasında olmalıdır.");

            var maskedCard = "**** **** **** " + dto.CardNumber[^4..];

            var transactionId = $"CARD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..6]}";

            // 8. Ödeme nesnesi oluştur
            var payment = new PaymentEntity
            {
                ReservationId = dto.ReservationId,
                Amount = dto.Amount,
                PaymentDate = DateTime.UtcNow,
                Type = PaymentType.CreditCard,
                Status = PaymentStatus.Completed,
                TransactionId = transactionId,
                CardHolderName = dto.CardHolderName,
                CardNumberMasked = maskedCard,
                ExpireMonth = dto.ExpireMonth,
                ExpireYear = dto.ExpireYear,
                InstallmentCount = dto.InstallmentCount ?? 1,
                CreatedByUserId = _currentUserService.UserId
            };

            await _paymentRepository.AddAsync(payment);

            // 9. Ödeme başarılıysa rezervasyon durumunu güncelle
            reservation.Status = ReservationStatus.Completed;
            reservation.ModifiedByUserId = _currentUserService.UserId;
            reservation.ModifiedDate = DateTime.UtcNow;
            await _reservationRepository.UpdateAsync(reservation);

            await _unitOfWork.SaveChangesAsync();

            // 10. DTO dön
            return new PaymentResponseDto
            {
                Id = payment.Id,
                ReservationId = payment.ReservationId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                Type = payment.Type,
                Status = payment.Status,
                TransactionId = payment.TransactionId,
                InstallmentCount = payment.InstallmentCount
            };
        }
    }
}
