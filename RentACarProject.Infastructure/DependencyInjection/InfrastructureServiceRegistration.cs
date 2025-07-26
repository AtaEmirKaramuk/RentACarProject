using Abstraction.Services;
using Infrastructure.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Infrastructure.Services;
using RentACarProject.Infrastructure.Services.Payments;

namespace RentACarProject.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IEmailService, SmtpEmailService>();
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            services.AddScoped<IPaymentStrategyFactory, PaymentStrategyFactory>();
            services.AddScoped<InternalBankTransferPaymentService>();
            services.AddScoped<InternalCardPaymentService>();

            return services;
        }
    }
}
