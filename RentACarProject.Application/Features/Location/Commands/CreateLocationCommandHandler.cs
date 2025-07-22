using MediatR;
using RentACarProject.Application.DTOs.Location;
using RentACarProject.Application.Features.Location.Commands;
using RentACarProject.Application.Abstraction.Repositories;
using DomainLocation = RentACarProject.Domain.Entities.Location;

namespace RentACarProject.Application.Features.Location.Commands
{
    public class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand, LocationResponseDto>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IUnitOfWork _unitOfWork; // 🔸 Eklenmeli

        public CreateLocationCommandHandler(ILocationRepository locationRepository, IUnitOfWork unitOfWork)
        {
            _locationRepository = locationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<LocationResponseDto> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Location;

            var location = new DomainLocation
            {
                LocationId = Guid.NewGuid(),
                Name = dto.Name,
                City = dto.City,
                Address = dto.Address,
                Description = dto.Description
            };

            await _locationRepository.AddAsync(location);
            await _unitOfWork.SaveChangesAsync(); // 🔥 Veritabanına yazmak için zorunlu

            return new LocationResponseDto
            {
                LocationId = location.LocationId,
                Name = location.Name,
                City = location.City,
                Address = location.Address,
                Description = location.Description
            };
        }
    }
}
