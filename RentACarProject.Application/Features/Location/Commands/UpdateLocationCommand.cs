using MediatR;
using RentACarProject.Application.DTOs.Location;

namespace RentACarProject.Application.Features.Location.Commands
{
    public class UpdateLocationCommand : IRequest<LocationResponseDto>
    {
        public UpdateLocationDto Location { get; set; }

        public UpdateLocationCommand(UpdateLocationDto location)
        {
            Location = location;
        }
    }
}
