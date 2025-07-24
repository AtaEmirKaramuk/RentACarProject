using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Application.Exceptions;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Domain.Entities;
using RentACarProject.Domain.Enums;
using PaymentEntity = RentACarProject.Domain.Entities.Payment; 

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, PaymentResponseDto>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreatePaymentCommandHandler(
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

        public async Task<PaymentResponseDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Payment;

            var reservation = await _reservationRepository.GetReservationByIdAsync(dto.ReservationId);
            if (reservation == null || reservation.IsDeleted)
                throw new NotFoundException("Rezervasyon bulunamadı.");

            if (reservation.Customer.UserId != _currentUserService.UserId)
                throw new ForbiddenAccessException("Bu rezervasyon size ait değil.");

            var payment = new PaymentEntity
            {
                PaymentId = Guid.NewGuid(),
                ReservationId = dto.ReservationId,
                Amount = dto.Amount,
                PaymentDate = DateTime.UtcNow,
                Type = dto.Type,
                Status = dto.Type == PaymentType.BankTransfer ? PaymentStatus.Pending : PaymentStatus.Completed,
                TransactionId = dto.TransactionId,
                SenderIban = dto.SenderIban,
                SenderName = dto.SenderName,
                CreatedDate = DateTime.UtcNow,
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
