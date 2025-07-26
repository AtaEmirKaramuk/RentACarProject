using RentACarProject.Domain.Enums;

namespace RentACarProject.Application.DTOs.Payment
{
    public class CreateBankTransferDto
    {
        public Guid ReservationId { get; set; }
        public decimal Amount { get; set; }

        // Gönderen bilgileri
        public string SenderIban { get; set; } = null!;
        public string SenderName { get; set; } = null!;

        public PaymentType Type { get; set; } = PaymentType.BankTransfer;
    }
}
