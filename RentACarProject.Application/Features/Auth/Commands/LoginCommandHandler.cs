using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Auth;
using RentACarProject.Application.Services;
using System.Security.Cryptography;
using System.Text;

namespace RentACarProject.Application.Features.Auth.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ServiceResponse<LoginResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenService _jwtTokenService;

        public LoginCommandHandler(IUserRepository userRepository, JwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<ServiceResponse<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUserNameAsync(request.UserName);
            if (user == null)
            {
                return new ServiceResponse<LoginResponseDto>
                {
                    Success = false,
                    Message = "Kullanıcı bulunamadı.",
                    Code = "404"
                };
            }

            var hashedPassword = HashPassword(request.Password);
            if (user.PasswordHash != hashedPassword)
            {
                return new ServiceResponse<LoginResponseDto>
                {
                    Success = false,
                    Message = "Şifre yanlış.",
                    Code = "400"
                };
            }

            var token = _jwtTokenService.GenerateToken(user);

            return new ServiceResponse<LoginResponseDto>
            {
                Success = true,
                Message = "Giriş başarılı.",
                Code = "200",
                Data = new LoginResponseDto
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Email = user.Email ?? "",
                    Role = user.Role.ToString(), // ✅ Enum string olarak dönüştürülüyor
                    Token = token
                }
            };
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
