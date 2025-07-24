using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Domain.Enums;

namespace RentACarProject.Infrastructure.Services.Payments
{
    public class PaymentStrategyFactory : IPaymentStrategyFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PaymentStrategyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPaymentStrategyService GetStrategy(PaymentType type)
        {
            return type switch
            {
                PaymentType.BankTransfer => (IPaymentStrategyService)_serviceProvider.GetService(typeof(BankTransferPaymentService))!,
                PaymentType.CreditCard => (IPaymentStrategyService)_serviceProvider.GetService(typeof(IyzicoPaymentService))!,
                _ => throw new NotImplementedException($"Ödeme tipi desteklenmiyor: {type}")
            };
        }
    }
}
