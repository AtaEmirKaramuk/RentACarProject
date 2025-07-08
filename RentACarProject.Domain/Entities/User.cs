using RentACarProject.Domain.Common;
using System;

namespace RentACarProject.Domain.Entities
{
    public class User : BaseEntity
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? Email { get; set; }
        public string? Role { get; set; }

        public Customer? Customer { get; set; }
    }
}
