using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Abstraction.Services
{
    public interface IPaymentStrategyService<TDto>
    {
        Task<PaymentResponseDto> ProcessPaymentAsync(TDto dto);
    }
}
