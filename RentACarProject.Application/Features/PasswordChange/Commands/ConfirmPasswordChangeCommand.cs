using Application.Features.PasswordChange.Dtos;
using MediatR;

namespace Application.Features.PasswordChange.Commands.ConfirmPasswordChange
{
    public class ConfirmPasswordChangeCommand : IRequest<PasswordChangeResponseDto>
    {
        public string VerificationCode { get; set; } = null!;
    }
}
