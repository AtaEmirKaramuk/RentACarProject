using RentACarProject.Domain.Enums;

namespace RentACarProject.Application.Abstraction.Services
{
    public interface IPaymentStrategyFactory
    {
        IPaymentStrategyService<TDto> GetStrategy<TDto>(PaymentType type);
    }
}
