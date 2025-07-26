using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.Common;
using RentACarProject.Application.Exceptions;
using RentACarProject.Domain.Enums;
using ReservationEntity = RentACarProject.Domain.Entities.Reservation;

namespace RentACarProject.Application.Features.Reservation.Commands
{
    public class CancelReservationCommandHandler : IRequestHandler<CancelReservationCommand, ServiceResponse<string>>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CancelReservationCommandHandler(
            IReservationRepository reservationRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _reservationRepository = reservationRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ServiceResponse<string>> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _reservationRepository.GetAsync(r => r.Id == request.Id && !r.IsDeleted);

            if (reservation == null)
                throw new NotFoundException("Rezervasyon bulunamadı.");

            if (!_currentUserService.UserId.HasValue || reservation.UserId != _currentUserService.UserId)
                throw new ForbiddenAccessException("Bu rezervasyona erişim izniniz yok.");

            if (reservation.Status == ReservationStatus.Cancelled)
                throw new BusinessException("Rezervasyon zaten iptal edilmiş.");

            if (reservation.Status == ReservationStatus.Completed)
                throw new BusinessException("Tamamlanmış bir rezervasyon iptal edilemez.");

            reservation.Status = ReservationStatus.Cancelled;

            await _unitOfWork.SaveChangesAsync();

            return new ServiceResponse<string>
            {
                Success = true,
                Message = "Rezervasyon başarıyla iptal edildi.",
                Data = reservation.Id.ToString()
            };
        }
    }
}
