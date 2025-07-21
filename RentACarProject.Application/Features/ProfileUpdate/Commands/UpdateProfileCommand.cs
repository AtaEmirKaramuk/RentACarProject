using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.ProfileUpdate;

namespace RentACarProject.Application.Features.ProfileUpdate.Commands
{
    public class UpdateProfileCommand : IRequest<ServiceResponse<UpdateProfileDto>>
    {
        public UpdateProfileDto Profile { get; set; } = null!;
    }
}
