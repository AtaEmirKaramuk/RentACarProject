using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Auth;

namespace RentACarProject.Application.Features.Auth.Commands
{
    public class RegisterCommand : IRequest<ServiceResponse<RegisterResponseDto>>
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Phone { get; set; }
    }
}
