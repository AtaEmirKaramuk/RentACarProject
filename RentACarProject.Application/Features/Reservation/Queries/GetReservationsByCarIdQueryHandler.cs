using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Reservation;

namespace RentACarProject.Application.Features.Reservation.Queries
{
    public class GetReservationsByCarIdQueryHandler : IRequestHandler<GetReservationsByCarIdQuery, ServiceResponse<List<ReservationResponseDto>>>
    {
        private readonly IReservationRepository _reservationRepository;

        public GetReservationsByCarIdQueryHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<ServiceResponse<List<ReservationResponseDto>>> Handle(GetReservationsByCarIdQuery request, CancellationToken cancellationToken)
        {
            var reservations = await _reservationRepository.GetReservationsByCarIdAsync(request.CarId);

            var result = reservations.Select(r => new ReservationResponseDto
            {
                Id = r.Id,
                CarPlate = r.Car?.Plate ?? "-",
                CarModel = r.Car?.Model?.Name ?? "-",
                CarBrand = r.Car?.Model?.Brand?.Name ?? "-",
                PickupLocation = r.PickupLocation?.Name ?? "-",
                DropoffLocation = r.DropoffLocation?.Name ?? "-",
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                TotalPrice = r.TotalPrice,
                Status = r.Status
            }).ToList();

            return new ServiceResponse<List<ReservationResponseDto>>
            {
                Success = true,
                Message = "Araca ait rezervasyonlar listelendi.",
                Data = result
            };
        }
    }
}
