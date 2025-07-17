using Microsoft.Extensions.Configuration;
using RentACarProject.Domain.Entities;
using RentACarProject.Domain.Enums;
using RentACarProject.Persistence.Context;
using System.Security.Cryptography;
using System.Text;

namespace RentACarProject.Persistence.Seed
{
    public static class SeedData
    {
        public static void SeedAdminUser(RentACarDbContext context, IConfiguration configuration)
        {
            if (context.Users.Any(u => u.Role == UserRole.Admin))
                return;

            // appsettings'den bilgileri al
            var adminSection = configuration.GetSection("AdminUser");
            var username = adminSection["UserName"];
            var email = adminSection["Email"];
            var password = adminSection["Password"];

            // Null veya boş kontrolü
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Admin user credentials are not properly configured in appsettings.json!");
            }

            // Şifre hash
            string HashPassword(string password)
            {
                using var sha256 = SHA256.Create();
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }

            var adminUser = new User
            {
                UserId = Guid.NewGuid(),
                UserName = username,
                Email = email,
                PasswordHash = HashPassword(password),
                Role = UserRole.Admin
            };

            context.Users.Add(adminUser);
            context.SaveChanges();
        }
    }
}
