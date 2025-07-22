using MediatR;

namespace RentACarProject.Application.Features.Location.Commands
{
    public class DeleteLocationCommand : IRequest<bool>
    {
        public Guid LocationId { get; set; }

        public DeleteLocationCommand(Guid locationId)
        {
            LocationId = locationId;
        }
    }
}
