using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Application.Exceptions;
using RentACarProject.Domain.Entities;
using RentACarProject.Domain.Enums;
using PaymentEntity = RentACarProject.Domain.Entities.Payment;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, PaymentResponseDto>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdatePaymentCommandHandler(
            IPaymentRepository paymentRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PaymentResponseDto> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Payment;

            var payment = await _paymentRepository.GetPaymentByIdAsync(dto.Id);
            if (payment == null || payment.IsDeleted)
                throw new NotFoundException("Ödeme kaydı bulunamadı.");

            if (payment.Reservation.Customer.UserId != _currentUserService.UserId)
                throw new ForbiddenAccessException("Bu ödeme kaydını güncelleme yetkiniz yok.");

            payment.Status = dto.Status;

            if (!string.IsNullOrWhiteSpace(dto.TransactionId))
                payment.TransactionId = dto.TransactionId;

            payment.ModifiedByUserId = _currentUserService.UserId;
            payment.ModifiedDate = DateTime.UtcNow;

            await _paymentRepository.UpdateAsync(payment);
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
