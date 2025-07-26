using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Abstraction.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> CreateCardPaymentAsync(CreateCardPaymentDto dto);
        Task<PaymentResponseDto> CreateBankTransferAsync(CreateBankTransferDto dto);
    }
}
