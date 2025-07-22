using MediatR;
using RentACarProject.Application.DTOs.Location;

namespace RentACarProject.Application.Features.Location.Queries
{
    public class GetAllLocationsQuery : IRequest<List<LocationResponseDto>>
    {
    }
}
