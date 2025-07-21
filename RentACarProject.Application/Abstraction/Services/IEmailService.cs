using System.Threading.Tasks;

namespace Abstraction.Services
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);
    }
}
