using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Reservation;
using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Application.Exceptions;

namespace RentACarProject.Application.Features.Reservation.Queries
{
    public class GetReservationWithPaymentsQueryHandler : IRequestHandler<GetReservationWithPaymentsQuery, ServiceResponse<ReservationWithPaymentsDto>>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IPaymentRepository _paymentRepository;

        public GetReservationWithPaymentsQueryHandler(
            IReservationRepository reservationRepository,
            IPaymentRepository paymentRepository)
        {
            _reservationRepository = reservationRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<ServiceResponse<ReservationWithPaymentsDto>> Handle(GetReservationWithPaymentsQuery request, CancellationToken cancellationToken)
        {
            var reservation = await _reservationRepository.GetByIdWithDetailsAsync(request.ReservationId);

            if (reservation == null)
                throw new NotFoundException("Rezervasyon bulunamadı.");

            var payments = await _paymentRepository.GetPaymentsByReservationIdAsync(reservation.Id);

            var dto = new ReservationWithPaymentsDto
            {
                Id = reservation.Id,
                CarPlate = reservation.Car?.Plate ?? "-",
                CarModel = reservation.Car?.Model?.Name ?? "-",
                CarBrand = reservation.Car?.Model?.Brand?.Name ?? "-",
                PickupLocation = reservation.PickupLocation?.Name ?? "-",
                DropoffLocation = reservation.DropoffLocation?.Name ?? "-",
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                TotalPrice = reservation.TotalPrice,
                Status = reservation.Status,
                Payments = payments.Select(p => new PaymentResponseDto
                {
                    Id = p.Id,
                    ReservationId = p.ReservationId,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    Status = p.Status,
                    Type = p.Type,
                    TransactionId = p.TransactionId,
                    SenderIban = p.SenderIban,
                    SenderName = p.SenderName
                }).ToList()
            };

            return new ServiceResponse<ReservationWithPaymentsDto>
            {
                Success = true,
                Message = "Rezervasyon ve ödemeleri başarıyla getirildi.",
                Data = dto
            };
        }
    }
}
