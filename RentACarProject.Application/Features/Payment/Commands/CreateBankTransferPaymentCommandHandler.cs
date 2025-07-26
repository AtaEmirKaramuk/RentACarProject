using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Application.Exceptions;
using RentACarProject.Domain.Enums;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class CreateBankTransferPaymentCommandHandler : IRequestHandler<CreateBankTransferPaymentCommand, PaymentResponseDto>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPaymentStrategyFactory _paymentStrategyFactory;

        public CreateBankTransferPaymentCommandHandler(
            IReservationRepository reservationRepository,
            ICurrentUserService currentUserService,
            IPaymentStrategyFactory paymentStrategyFactory)
        {
            _reservationRepository = reservationRepository;
            _currentUserService = currentUserService;
            _paymentStrategyFactory = paymentStrategyFactory;
        }

        public async Task<PaymentResponseDto> Handle(CreateBankTransferPaymentCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Payment;

            var reservation = await _reservationRepository.GetReservationByIdAsync(dto.ReservationId);
            if (reservation == null || reservation.IsDeleted)
                throw new NotFoundException("Rezervasyon bulunamadı.");

            if (reservation.Customer.UserId != _currentUserService.UserId)
                throw new ForbiddenAccessException("Bu rezervasyon size ait değil.");

            var strategy = _paymentStrategyFactory.GetStrategy<CreateBankTransferDto>(PaymentType.BankTransfer);
            var result = await strategy.ProcessPaymentAsync(dto);

            return result;
        }
    }
}
