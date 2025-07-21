using Application.Features.PasswordChange.Dtos;
using MediatR;

namespace Application.Features.PasswordChange.Commands.InitiatePasswordChange
{
    public class InitiatePasswordChangeCommand : IRequest<PasswordChangeResponseDto>
    {
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string ConfirmNewPassword { get; set; } = null!;
    }
}
