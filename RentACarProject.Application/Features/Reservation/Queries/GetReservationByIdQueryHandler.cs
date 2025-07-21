using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Reservation;
using RentACarProject.Application.Exceptions;

namespace RentACarProject.Application.Features.Reservation.Queries
{
    public class GetReservationByIdQueryHandler : IRequestHandler<GetReservationByIdQuery, ServiceResponse<ReservationResponseDto>>
    {
        private readonly IReservationRepository _reservationRepository;

        public GetReservationByIdQueryHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<ServiceResponse<ReservationResponseDto>> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
        {
            var reservation = await _reservationRepository.GetByIdWithDetailsAsync(request.Id);

            if (reservation == null)
                throw new BusinessException("Rezervasyon bulunamadı.");

            var dto = new ReservationResponseDto
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
            };

            return new ServiceResponse<ReservationResponseDto>
            {
                Success = true,
                Message = "Rezervasyon detayları getirildi.",
                Data = dto
            };
        }
    }
}
