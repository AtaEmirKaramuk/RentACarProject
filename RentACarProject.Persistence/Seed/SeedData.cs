using RentACarProject.Domain.Entities;
using RentACarProject.Persistence.Context;
using System.Security.Cryptography;
using System.Text;

namespace RentACarProject.Persistence.Seed
{
    public static class SeedData
    {
        public static void SeedAdminUser(RentACarDbContext context)
        {
            // Eğer admin varsa tekrar ekleme
            if (context.Users.Any(u => u.Role == Domain.Enums.UserRole.Admin))
            {
                return;
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
                UserName = "AtaEmirIronside",
                Email = "admin@rentacar.com",
                PasswordHash = HashPassword("Aaei092002!"), 
                Role = Domain.Enums.UserRole.Admin
            };

            context.Users.Add(adminUser);
            context.SaveChanges();
        }
    }
}
