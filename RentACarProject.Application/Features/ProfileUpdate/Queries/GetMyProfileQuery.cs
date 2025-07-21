using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.ProfileUpdate;

namespace RentACarProject.Application.Features.ProfileUpdate.Queries
{
    public class GetMyProfileQuery : IRequest<ServiceResponse<UpdateProfileDto>>
    {
    }
}
