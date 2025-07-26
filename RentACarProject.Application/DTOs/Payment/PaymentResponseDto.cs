using RentACarProject.Domain.Enums;

public class PaymentResponseDto
{
    public Guid Id { get; set; }
    public Guid ReservationId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }

    public PaymentType Type { get; set; }
    public PaymentStatus Status { get; set; }

    public string? TransactionId { get; set; }

    // Havale bilgileri
    public string? SenderIban { get; set; }
    public string? SenderName { get; set; }

    // Kredi kartı
    public int? InstallmentCount { get; set; }
}
