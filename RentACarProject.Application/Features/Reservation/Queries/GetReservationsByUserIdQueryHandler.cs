using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Reservation;
using RentACarProject.Application.Exceptions;

namespace RentACarProject.Application.Features.Reservation.Queries
{
    public class GetReservationsByUserIdQueryHandler : IRequestHandler<GetReservationsByUserIdQuery, ServiceResponse<List<ReservationResponseDto>>>
    {
        private readonly IReservationRepository _reservationRepository;

        public GetReservationsByUserIdQueryHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<ServiceResponse<List<ReservationResponseDto>>> Handle(GetReservationsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var reservations = await _reservationRepository.GetReservationsByUserIdAsync(request.UserId);

            if (reservations == null || reservations.Count == 0)
                throw new BusinessException("Bu kullanıcıya ait rezervasyon bulunamadı.");

            var result = reservations.Select(reservation => new ReservationResponseDto
            {
                Id = reservation.Id,
                CarPlate = reservation.Car.Plate,
                CarModel = reservation.Car.Model.Name,
                CarBrand = reservation.Car.Model.Brand.Name,
                PickupLocation = reservation.PickupLocation.Name,
                DropoffLocation = reservation.DropoffLocation.Name,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                TotalPrice = reservation.TotalPrice,
                Status = reservation.Status
            }).ToList();

            return new ServiceResponse<List<ReservationResponseDto>>
            {
                Success = true,
                Message = "Kullanıcının rezervasyonları listelendi.",
                Data = result
            };
        }
    }
}
