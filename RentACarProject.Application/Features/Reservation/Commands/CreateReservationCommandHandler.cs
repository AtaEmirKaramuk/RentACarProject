using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Reservation;
using RentACarProject.Application.Exceptions;
using RentACarProject.Domain.Enums;
using ReservationEntity = RentACarProject.Domain.Entities.Reservation;

namespace RentACarProject.Application.Features.Reservation.Commands
{
    public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, ServiceResponse<ReservationResponseDto>>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICarRepository _carRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateReservationCommandHandler(
            IReservationRepository reservationRepository,
            ICarRepository carRepository,
            ILocationRepository locationRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _reservationRepository = reservationRepository;
            _carRepository = carRepository;
            _locationRepository = locationRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ServiceResponse<ReservationResponseDto>> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Reservation;

            var car = await _carRepository.GetCarWithModelAndBrandAsync(dto.CarId);
            if (car == null)
                throw new BusinessException("Araç bulunamadı.");

            var pickupLocation = await _locationRepository.GetAsync(l => l.LocationId == dto.PickupLocationId && !l.IsDeleted);
            if (pickupLocation == null)
                throw new BusinessException("Alış lokasyonu bulunamadı.");

            var dropoffLocation = await _locationRepository.GetAsync(l => l.LocationId == dto.DropoffLocationId && !l.IsDeleted);
            if (dropoffLocation == null)
                throw new BusinessException("Teslim lokasyonu bulunamadı.");

            if (dto.StartDate >= dto.EndDate)
                throw new BusinessException("Rezervasyon süresi en az 1 gün olmalıdır.");

            // 🚨 Rezervasyon çakışma kontrolü
            var conflictingReservations = await _reservationRepository.GetReservationsByCarIdAsync(dto.CarId);
            var hasConflict = conflictingReservations.Any(r =>
                r.Status == ReservationStatus.Active &&
                r.StartDate < dto.EndDate &&
                r.EndDate > dto.StartDate);

            if (hasConflict)
                throw new BusinessException("Belirtilen tarihlerde bu araç zaten rezerve edilmiştir.");

            var totalDays = (dto.EndDate.Date - dto.StartDate.Date).Days;
            var totalPrice = car.DailyPrice * totalDays;

            if (!_currentUserService.UserId.HasValue)
                throw new BusinessException("Kullanıcı kimliği alınamadı.");

            var reservation = new ReservationEntity
            {
                Id = Guid.NewGuid(),
                CarId = dto.CarId,
                UserId = _currentUserService.UserId.Value,
                PickupLocationId = dto.PickupLocationId,
                DropoffLocationId = dto.DropoffLocationId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                TotalPrice = totalPrice,
                Status = ReservationStatus.Active
            };

            await _reservationRepository.AddAsync(reservation);
            await _unitOfWork.SaveChangesAsync();

            var responseDto = new ReservationResponseDto
            {
                Id = reservation.Id,
                CarPlate = car.Plate,
                CarModel = car.Model.Name,
                CarBrand = car.Model.Brand.Name,
                PickupLocation = pickupLocation.Name,
                DropoffLocation = dropoffLocation.Name,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                TotalPrice = reservation.TotalPrice,
                Status = reservation.Status
            };

            return new ServiceResponse<ReservationResponseDto>
            {
                Success = true,
                Message = "Rezervasyon başarıyla oluşturuldu.",
                Data = responseDto
            };
        }
    }
}
