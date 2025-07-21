using Abstraction.Services;
using Application.Features.PasswordChange.Commands.InitiatePasswordChange;
using Application.Features.PasswordChange.Dtos;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using System.Security.Cryptography;
using System.Text;
using System;

namespace Application.Features.PasswordChange.Handlers
{
    public class InitiatePasswordChangeCommandHandler : IRequestHandler<InitiatePasswordChangeCommand, PasswordChangeResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;
        private readonly ICurrentUserService _currentUserService;

        public InitiatePasswordChangeCommandHandler(
            IUserRepository userRepository,
            IMemoryCache cache,
            IEmailService emailService,
            ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _cache = cache;
            _emailService = emailService;
            _currentUserService = currentUserService;
        }

        public async Task<PasswordChangeResponseDto> Handle(InitiatePasswordChangeCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId == null || userId == Guid.Empty)
            {
                return new PasswordChangeResponseDto
                {
                    Success = false,
                    Message = "Geçersiz kullanıcı oturumu."
                };
            }

            var user = await _userRepository.GetByIdAsync(userId.Value);
            if (user == null)
            {
                return new PasswordChangeResponseDto
                {
                    Success = false,
                    Message = "Kullanıcı bulunamadı."
                };
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                return new PasswordChangeResponseDto
                {
                    Success = false,
                    Message = "Kullanıcının email bilgisi eksik."
                };
            }

            var hashedCurrentPassword = HashPassword(request.CurrentPassword);
            if (user.PasswordHash != hashedCurrentPassword)
            {
                return new PasswordChangeResponseDto
                {
                    Success = false,
                    Message = "Mevcut şifre hatalı."
                };
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return new PasswordChangeResponseDto
                {
                    Success = false,
                    Message = "Yeni şifreler eşleşmiyor."
                };
            }

            // Kod üret ve hashle
            var verificationCode = new Random().Next(100000, 999999).ToString();
            var newHashedPassword = HashPassword(request.NewPassword);

            _cache.Set($"pwd-change-code:{userId}", verificationCode, TimeSpan.FromMinutes(5));
            _cache.Set($"pwd-change-hash:{userId}", newHashedPassword, TimeSpan.FromMinutes(5));

            Console.WriteLine($"Şifre değişikliği maili gönderiliyor: {user.Email}");

            try
            {
                await _emailService.SendAsync(
                    to: user.Email,
                    subject: "RentACar Şifre Değiştirme Doğrulama Kodu",
                    body: $"Şifre değişikliğini tamamlamak için doğrulama kodunuz: {verificationCode}");
                Console.WriteLine("Şifre değişikliği maili gönderildi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mail gönderim hatası: {ex.Message}");
                throw;
            }

            return new PasswordChangeResponseDto
            {
                Success = true,
                Message = "Doğrulama kodu gönderildi. Lütfen emailinizi kontrol edin."
            };
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
