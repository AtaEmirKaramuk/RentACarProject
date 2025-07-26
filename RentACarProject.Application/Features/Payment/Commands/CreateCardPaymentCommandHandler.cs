using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Application.Exceptions;
using RentACarProject.Domain.Enums;
using PaymentEntity = RentACarProject.Domain.Entities.Payment;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class CreateCardPaymentCommandHandler : IRequestHandler<CreateCardPaymentCommand, PaymentResponseDto>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPaymentStrategyFactory _paymentStrategyFactory;

        public CreateCardPaymentCommandHandler(
            IReservationRepository reservationRepository,
            ICurrentUserService currentUserService,
            IPaymentStrategyFactory paymentStrategyFactory)
        {
            _reservationRepository = reservationRepository;
            _currentUserService = currentUserService;
            _paymentStrategyFactory = paymentStrategyFactory;
        }

        public async Task<PaymentResponseDto> Handle(CreateCardPaymentCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Payment;

            var reservation = await _reservationRepository.GetReservationByIdAsync(dto.ReservationId);
            if (reservation == null || reservation.IsDeleted)
                throw new NotFoundException("Rezervasyon bulunamadı.");

            if (reservation.Customer.UserId != _currentUserService.UserId)
                throw new ForbiddenAccessException("Bu rezervasyon size ait değil.");

            var strategy = _paymentStrategyFactory.GetStrategy<CreateCardPaymentDto>(PaymentType.CreditCard);
            var result = await strategy.ProcessPaymentAsync(dto);

            return result;
        }
    }
}
