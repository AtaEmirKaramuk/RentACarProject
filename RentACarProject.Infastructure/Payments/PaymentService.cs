using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Infrastructure.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentStrategyFactory _paymentStrategyFactory;

        public PaymentService(IPaymentStrategyFactory paymentStrategyFactory)
        {
            _paymentStrategyFactory = paymentStrategyFactory;
        }

        public async Task<PaymentResponseDto> CreateCardPaymentAsync(CreateCardPaymentDto dto)
        {
            var strategy = _paymentStrategyFactory.GetStrategy<CreateCardPaymentDto>(dto.Type);
            return await strategy.ProcessPaymentAsync(dto);
        }

        public async Task<PaymentResponseDto> CreateBankTransferAsync(CreateBankTransferDto dto)
        {
            var strategy = _paymentStrategyFactory.GetStrategy<CreateBankTransferDto>(dto.Type);
            return await strategy.ProcessPaymentAsync(dto);
        }
    }
}
