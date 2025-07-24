using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Domain.Enums;

namespace RentACarProject.Application.Abstraction.Services
{
    public interface IPaymentStrategyFactory
    {
        IPaymentStrategyService GetStrategy(PaymentType type);
    }
}
