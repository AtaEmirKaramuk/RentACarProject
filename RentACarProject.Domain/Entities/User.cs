using RentACarProject.Domain.Common;
using RentACarProject.Domain.Enums;

namespace RentACarProject.Domain.Entities
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? Email { get; set; }

        public UserRole Role { get; set; } = UserRole.User;

        public Customer? Customer { get; set; }
    }
}
