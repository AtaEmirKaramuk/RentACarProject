using RentACarProject.Domain.Enums;

namespace RentACarProject.Application.DTOs.Payment
{
    public class CreatePaymentDto
    {
        public Guid ReservationId { get; set; }
        public decimal Amount { get; set; }
        public PaymentType Type { get; set; }

        // Kredi Kartı için (iyzico'da kullanılacak)
        public string? CardHolderName { get; set; }
        public string? CardNumber { get; set; }
        public string? ExpireMonth { get; set; }
        public string? ExpireYear { get; set; }
        public string? Cvc { get; set; }

        // Banka Havalesi için
        public string? SenderIban { get; set; }
        public string? SenderName { get; set; }

        // Ortak alan: işlem referans kodu (iyzico veya banka)
        public string? TransactionId { get; set; }
    }
}
