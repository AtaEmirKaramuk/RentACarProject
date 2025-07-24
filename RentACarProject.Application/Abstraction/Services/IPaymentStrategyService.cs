using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Abstraction.Services
{
    public interface IPaymentStrategyService
    {
        Task<PaymentResponseDto> ProcessPaymentAsync(CreatePaymentDto dto);
    }
}
