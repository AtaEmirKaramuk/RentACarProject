using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Features.Payment.Queries
{
    public class GetMyPaymentsQueryHandler : IRequestHandler<GetMyPaymentsQuery, ServiceResponse<List<PaymentResponseDto>>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetMyPaymentsQueryHandler(IPaymentRepository paymentRepository, ICurrentUserService currentUserService)
        {
            _paymentRepository = paymentRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ServiceResponse<List<PaymentResponseDto>>> Handle(GetMyPaymentsQuery request, CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId == null)
                return new ServiceResponse<List<PaymentResponseDto>> { Success = false, Message = "Kullanıcı oturumu bulunamadı.", Code = "401" };

            var userId = _currentUserService.UserId.Value;

            var payments = await _paymentRepository.GetUserPaymentsWithFiltersAsync(
                userId,
                request.StartDate,
                request.EndDate,
                request.Status,
                request.Type
            );

            var result = payments.Select(p => new PaymentResponseDto
            {
                Id = p.Id,
                ReservationId = p.ReservationId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                Type = p.Type,
                Status = p.Status,
                TransactionId = p.TransactionId,
                SenderIban = p.SenderIban,
                SenderName = p.SenderName
            }).ToList();

            return new ServiceResponse<List<PaymentResponseDto>>
            {
                Success = true,
                Message = "Kullanıcının ödemeleri listelendi.",
                Data = result
            };
        }
    }
}
