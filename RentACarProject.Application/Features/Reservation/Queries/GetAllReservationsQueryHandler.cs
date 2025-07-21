using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Reservation;
using Microsoft.EntityFrameworkCore;

namespace RentACarProject.Application.Features.Reservation.Queries
{
    public class GetAllReservationsQueryHandler : IRequestHandler<GetAllReservationsQuery, ServiceResponse<List<ReservationResponseDto>>>
    {
        private readonly IReservationRepository _reservationRepository;

        public GetAllReservationsQueryHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<ServiceResponse<List<ReservationResponseDto>>> Handle(GetAllReservationsQuery request, CancellationToken cancellationToken)
        {
            var reservations = await _reservationRepository
                .GetAll()
                .Include(r => r.Car).ThenInclude(c => c.Model).ThenInclude(m => m.Brand)
                .Include(r => r.PickupLocation)
                .Include(r => r.DropoffLocation)
                .ToListAsync(cancellationToken);

            var dtoList = reservations.Select(r => new ReservationResponseDto
            {
                Id = r.Id,
                CarPlate = r.Car.Plate,
                CarModel = r.Car.Model.Name,
                CarBrand = r.Car.Model.Brand.Name,
                PickupLocation = r.PickupLocation.Name,
                DropoffLocation = r.DropoffLocation.Name,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                TotalPrice = r.TotalPrice,
                Status = r.Status
            }).ToList();

            return new ServiceResponse<List<ReservationResponseDto>>
            {
                Success = true,
                Message = "Tüm rezervasyonlar listelendi.",
                Data = dtoList
            };
        }
    }
}
