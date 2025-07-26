using RentACarProject.Domain.Entities;

namespace RentACarProject.Application.Abstraction.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
