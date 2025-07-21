using Application.Features.PasswordChange.Commands.ConfirmPasswordChange;
using Application.Features.PasswordChange.Dtos;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PasswordChange.Handlers
{
    public class ConfirmPasswordChangeCommandHandler : IRequestHandler<ConfirmPasswordChangeCommand, PasswordChangeResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private readonly ICurrentUserService _currentUserService;

        public ConfirmPasswordChangeCommandHandler(
            IUserRepository userRepository,
            IMemoryCache cache,
            ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _cache = cache;
            _currentUserService = currentUserService;
        }

        public async Task<PasswordChangeResponseDto> Handle(ConfirmPasswordChangeCommand request, CancellationToken cancellationToken)
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

            var cacheCodeKey = $"pwd-change-code:{userId}";
            var cacheHashKey = $"pwd-change-hash:{userId}";

            if (!_cache.TryGetValue(cacheCodeKey, out string? expectedCode) || expectedCode != request.VerificationCode)
            {
                return new PasswordChangeResponseDto
                {
                    Success = false,
                    Message = "Geçersiz ya da süresi dolmuş doğrulama kodu."
                };
            }

            if (!_cache.TryGetValue(cacheHashKey, out string? newHashedPassword))
            {
                return new PasswordChangeResponseDto
                {
                    Success = false,
                    Message = "Yeni şifre geçici olarak kaydedilemedi."
                };
            }

            if (newHashedPassword == null)
            {
                
                throw new Exception("Yeni şifre bilgisi bulunamadı.");
            }

            user.PasswordHash = newHashedPassword;


            await _userRepository.UpdateAsync(user);

            _cache.Remove(cacheCodeKey);
            _cache.Remove(cacheHashKey);

            return new PasswordChangeResponseDto
            {
                Success = true,
                Message = "Şifre başarıyla güncellendi."
            };
        }

        private static string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
