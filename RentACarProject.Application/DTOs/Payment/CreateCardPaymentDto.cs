using RentACarProject.Domain.Enums;

namespace RentACarProject.Application.DTOs.Payment
{
    public class CreateCardPaymentDto
    {
        public Guid ReservationId { get; set; }
        public decimal Amount { get; set; }

        public string CardHolderName { get; set; } = null!;
        public string CardNumber { get; set; } = null!;
        public int ExpireMonth { get; set; }
        public int ExpireYear { get; set; }
        public string Cvc { get; set; } = null!;

        public int? InstallmentCount { get; set; }

        public PaymentType Type { get; set; } = PaymentType.CreditCard;
    }
}
