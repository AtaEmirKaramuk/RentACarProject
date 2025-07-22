using MediatR;
using RentACarProject.Application.DTOs.Location;

namespace RentACarProject.Application.Features.Location.Commands
{
    public class CreateLocationCommand : IRequest<LocationResponseDto>
    {
        public CreateLocationDto Location { get; set; }

        public CreateLocationCommand(CreateLocationDto location)
        {
            Location = location;
        }
    }
}
