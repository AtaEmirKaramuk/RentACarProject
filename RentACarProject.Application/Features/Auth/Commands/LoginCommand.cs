using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Auth;

namespace RentACarProject.Application.Features.Auth.Commands
{
    public class LoginCommand : IRequest<ServiceResponse<LoginResponseDto>>
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
