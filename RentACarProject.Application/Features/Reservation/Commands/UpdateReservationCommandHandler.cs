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
    public class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand, ServiceResponse<ReservationResponseDto>>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICarRepository _carRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateReservationCommandHandler(
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

        public async Task<ServiceResponse<ReservationResponseDto>> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Reservation;

            var reservation = await _reservationRepository.GetAsync(r => r.Id == dto.Id && !r.IsDeleted);
            if (reservation == null)
                throw new NotFoundException("Rezervasyon bulunamadı.");

            if (reservation.UserId != _currentUserService.UserId)
                throw new ForbiddenAccessException("Bu rezervasyonu güncelleme yetkiniz yok.");

            // Navigation property'ler için include'lu özel sorgu kullanıyoruz
            var car = await _carRepository.GetCarWithModelAndBrandAsync(dto.CarId);
            if (car == null || car.IsDeleted)
                throw new BusinessException("Araç bulunamadı.");

            var pickup = await _locationRepository.GetAsync(l => l.LocationId == dto.PickupLocationId && !l.IsDeleted)
                        ?? throw new BusinessException("Alış lokasyonu bulunamadı.");

            var dropoff = await _locationRepository.GetAsync(l => l.LocationId == dto.DropoffLocationId && !l.IsDeleted)
                         ?? throw new BusinessException("Teslim lokasyonu bulunamadı.");

            if (dto.StartDate >= dto.EndDate)
                throw new BusinessException("Başlangıç tarihi bitiş tarihinden büyük veya eşit olamaz.");

            var totalDays = (dto.EndDate.Date - dto.StartDate.Date).Days;
            if (totalDays <= 0)
                throw new BusinessException("Rezervasyon süresi en az 1 gün olmalıdır.");

            // Çakışma kontrolü (başka bir aktif rezervasyonla)
            var hasConflict = (await _reservationRepository.GetReservationsByCarIdAsync(dto.CarId))
                .Any(r =>
                    r.Id != reservation.Id &&
                    r.Status == ReservationStatus.Active &&
                    r.StartDate < dto.EndDate &&
                    r.EndDate > dto.StartDate);

            if (hasConflict)
                throw new BusinessException("Belirtilen tarihlerde bu araç başka bir rezervasyonda.");

            // Güncelleme
            reservation.CarId = dto.CarId;
            reservation.PickupLocationId = dto.PickupLocationId;
            reservation.DropoffLocationId = dto.DropoffLocationId;
            reservation.StartDate = dto.StartDate;
            reservation.EndDate = dto.EndDate;
            reservation.TotalPrice = car.DailyPrice * totalDays;

            await _unitOfWork.SaveChangesAsync();

            var response = new ReservationResponseDto
            {
                Id = reservation.Id,
                CarPlate = car.Plate,
                CarModel = car.Model.Name,
                CarBrand = car.Model.Brand.Name,
                PickupLocation = pickup.Name,
                DropoffLocation = dropoff.Name,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                TotalPrice = reservation.TotalPrice,
                Status = reservation.Status
            };

            return new ServiceResponse<ReservationResponseDto>
            {
                Success = true,
                Message = "Rezervasyon güncellendi.",
                Data = response
            };
        }
    }
}
