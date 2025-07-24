using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Abstraction.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> CreatePaymentAsync(CreatePaymentDto dto);
    }
}
