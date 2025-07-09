using RentACarProject.Domain.Common;
using RentACarProject.Domain.Enums;
using System;

namespace RentACarProject.Domain.Entities
{
    public class User : BaseEntity
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? Email { get; set; }

        // Enum olarak saklıyoruz, default User
        public UserRole Role { get; set; } = UserRole.User;

        public Customer? Customer { get; set; }
    }
}
