using RentACarProject.Domain.Enums;

namespace RentACarProject.Application.DTOs.Payment
{
    public class UpdatePaymentDto
    {
        public Guid PaymentId { get; set; }
        public PaymentStatus Status { get; set; }
        public string? TransactionId { get; set; } // iyzico/havale referans no
    }
}
