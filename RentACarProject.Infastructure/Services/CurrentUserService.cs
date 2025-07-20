using Microsoft.AspNetCore.Http;
using RentACarProject.Application.Abstraction.Services;
using System.Security.Claims;

namespace RentACarProject.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var value = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return Guid.TryParse(value, out var id) ? id : null;
            }
        }

        public string? UserName =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

        public string? Email =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

        public string? Role =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}
