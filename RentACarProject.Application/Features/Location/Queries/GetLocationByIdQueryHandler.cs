using MediatR;
using RentACarProject.Application.DTOs.Location;
using RentACarProject.Application.Features.Location.Queries;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Exceptions;

namespace RentACarProject.Application.Features.Location.Queries
{
    public class GetLocationByIdQueryHandler : IRequestHandler<GetLocationByIdQuery, LocationResponseDto>
    {
        private readonly ILocationRepository _locationRepository;

        public GetLocationByIdQueryHandler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<LocationResponseDto> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
        {
            var location = await _locationRepository.GetLocationByIdAsync(request.LocationId);

            if (location == null || location.IsDeleted)
                throw new NotFoundException("Lokasyon bulunamadı.");

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
