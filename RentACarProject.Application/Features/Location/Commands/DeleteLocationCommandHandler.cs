using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Features.Location.Commands;
using RentACarProject.Application.Exceptions;

namespace RentACarProject.Application.Features.Location.Commands
{
    public class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand, bool>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLocationCommandHandler(
            ILocationRepository locationRepository,
            IUnitOfWork unitOfWork)
        {
            _locationRepository = locationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
        {
            var location = await _locationRepository.GetLocationByIdAsync(request.LocationId);

            if (location == null || location.IsDeleted)
                throw new NotFoundException("Lokasyon bulunamadı.");

            location.IsDeleted = true;

            await _locationRepository.UpdateAsync(location);
            await _unitOfWork.SaveChangesAsync(); // 🔥 Eksik olan kısım

            return true;
        }
    }
}
