using MediatR;
using RentACarProject.Application.DTOs.Location;
using RentACarProject.Application.Features.Location.Commands;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Exceptions;
using RentACarProject.Domain.Entities;

namespace RentACarProject.Application.Features.Location.Commands
{
    public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, LocationResponseDto>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateLocationCommandHandler(
            ILocationRepository locationRepository,
            IUnitOfWork unitOfWork)
        {
            _locationRepository = locationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<LocationResponseDto> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Location;

            var location = await _locationRepository.GetLocationByIdAsync(dto.LocationId);

            if (location == null || location.IsDeleted)
                throw new NotFoundException("Lokasyon bulunamadı.");

            location.Name = dto.Name;
            location.City = dto.City;
            location.Address = dto.Address;
            location.Description = dto.Description;

            await _locationRepository.UpdateAsync(location);
            await _unitOfWork.SaveChangesAsync(); 

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
