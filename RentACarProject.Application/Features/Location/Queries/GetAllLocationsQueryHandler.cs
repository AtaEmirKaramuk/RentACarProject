using MediatR;
using RentACarProject.Application.DTOs.Location;
using RentACarProject.Application.Abstraction.Repositories;

namespace RentACarProject.Application.Features.Location.Queries
{
    public class GetAllLocationsQueryHandler : IRequestHandler<GetAllLocationsQuery, List<LocationResponseDto>>
    {
        private readonly ILocationRepository _locationRepository;

        public GetAllLocationsQueryHandler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<List<LocationResponseDto>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
        {
            var locations = await _locationRepository.GetAllLocationsAsync();

            return locations
                .Where(x => !x.IsDeleted)
                .Select(loc => new LocationResponseDto
                {
                    Id = loc.Id,
                    Name = loc.Name,
                    City = loc.City,
                    Address = loc.Address,
                    Description = loc.Description
                })
                .ToList();
        }
    }
}
