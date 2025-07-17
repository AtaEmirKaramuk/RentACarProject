//using Microsoft.AspNetCore.Http;
//using System.Security.Claims;
//using RentACarProject.Application.Abstraction.Services;

//namespace RentACarProject.Infrastructure.Services
//{
//    public class CurrentUserService : ICurrentUserService
//    {
//        public Guid? UserId { get; }
//        public string? UserName { get; }
//        public string? Email { get; }
//        public string? Role { get; }
//        public bool IsAuthenticated { get; }

//        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
//        {
//            var user = httpContextAccessor.HttpContext?.User;

//            if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
//            {
//                IsAuthenticated = true;
//                UserId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
//                UserName = user.FindFirstValue(ClaimTypes.Name);
//                Email = user.FindFirstValue(ClaimTypes.Email);
//                Role = user.FindFirstValue(ClaimTypes.Role);
//            }
//        }
//    }
//}
