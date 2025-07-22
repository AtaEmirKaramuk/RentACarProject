using MediatR;
using RentACarProject.Application.DTOs.Location;

namespace RentACarProject.Application.Features.Location.Queries
{
    public class GetLocationByIdQuery : IRequest<LocationResponseDto>
    {
        public Guid LocationId { get; set; }

        public GetLocationByIdQuery(Guid locationId)
        {
            LocationId = locationId;
        }
    }
}
