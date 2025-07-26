using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Application.Exceptions;

namespace RentACarProject.Application.Features.Payment.Queries
{
    public class GetMyPaymentDetailQueryHandler : IRequestHandler<GetMyPaymentDetailQuery, ServiceResponse<PaymentResponseDto>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetMyPaymentDetailQueryHandler(IPaymentRepository paymentRepository, ICurrentUserService currentUserService)
        {
            _paymentRepository = paymentRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ServiceResponse<PaymentResponseDto>> Handle(GetMyPaymentDetailQuery request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(request.PaymentId);

            if (payment == null || payment.IsDeleted)
                return new ServiceResponse<PaymentResponseDto> { Success = false, Message = "Ödeme bulunamadı.", Code = "404" };

            if (payment.Reservation.Customer.UserId != _currentUserService.UserId)
                throw new ForbiddenAccessException("Bu ödeme kaydına erişim yetkiniz yok.");

            var dto = new PaymentResponseDto
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

            return new ServiceResponse<PaymentResponseDto>
            {
                Success = true,
                Message = "Ödeme detayları getirildi.",
                Data = dto
            };
        }
    }
}
