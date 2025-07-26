using Microsoft.Extensions.DependencyInjection;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Persistence.Repositories;

namespace RentACarProject.Persistence.DependencyInjection
{
    public static class RepositoryServiceRegistration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, EfUserRepository>();
            services.AddScoped<ICustomerRepository, EfCustomerRepository>();
            services.AddScoped<ICarRepository, EfCarRepository>();
            services.AddScoped<IBrandRepository, EfBrandRepository>();
            services.AddScoped<IModelRepository, EfModelRepository>();
            services.AddScoped<IReservationRepository, EfReservationRepository>();
            services.AddScoped<IPaymentRepository, EfPaymentRepository>();
            services.AddScoped<ILocationRepository, EfLocationRepository>();

            return services;
        }
    }
}
