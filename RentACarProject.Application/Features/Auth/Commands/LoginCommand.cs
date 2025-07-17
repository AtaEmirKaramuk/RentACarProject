using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Domain.DTOs.Auth;

namespace RentACarProject.Application.Features.Auth.Commands
{
    public class LoginCommand : IRequest<ServiceResponse<LoginResponseDto>>
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
