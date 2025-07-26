using Microsoft.Extensions.DependencyInjection;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Domain.Enums;
using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Infrastructure.Services.Payments
{
    public class PaymentStrategyFactory : IPaymentStrategyFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PaymentStrategyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPaymentStrategyService<TDto> GetStrategy<TDto>(PaymentType type)
        {
            return type switch
            {
                PaymentType.CreditCard when typeof(TDto) == typeof(CreateCardPaymentDto)
                    => (IPaymentStrategyService<TDto>)_serviceProvider.GetRequiredService(typeof(InternalCardPaymentService)),

                PaymentType.BankTransfer when typeof(TDto) == typeof(CreateBankTransferDto)
                    => (IPaymentStrategyService<TDto>)_serviceProvider.GetRequiredService(typeof(InternalBankTransferPaymentService)),

                _ => throw new NotImplementedException($"Ödeme tipi desteklenmiyor veya DTO tipi uyumsuz: {type}")
            };
        }
    }
}
