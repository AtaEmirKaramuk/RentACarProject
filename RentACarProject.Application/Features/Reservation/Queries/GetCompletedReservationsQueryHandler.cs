using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Reservation;

namespace RentACarProject.Application.Features.Reservation.Queries
{
    public class GetCompletedReservationsQueryHandler : IRequestHandler<GetCompletedReservationsQuery, ServiceResponse<List<ReservationResponseDto>>>
    {
        private readonly IReservationRepository _reservationRepository;

        public GetCompletedReservationsQueryHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<ServiceResponse<List<ReservationResponseDto>>> Handle(GetCompletedReservationsQuery request, CancellationToken cancellationToken)
        {
            var reservations = await _reservationRepository.GetCompletedReservationsAsync();

            var result = reservations.Select(r => new ReservationResponseDto
            {
                Id = r.Id,
                CarPlate = r.Car.Plate,
                CarModel = r.Car.Model.Name,
                CarBrand = r.Car.Model.Brand.Name,
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
                Message = "Tamamlanmış rezervasyonlar listelendi.",
                Data = result
            };
        }
    }
}
