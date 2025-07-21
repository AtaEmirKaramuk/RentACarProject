namespace Application.Features.PasswordChange.Dtos
{
    public class PasswordChangeResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
    }
}
