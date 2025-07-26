using RentACarProject.Domain.Enums;

public class ReservationWithPaymentsDto
{
    public Guid Id { get; set; }
    public string CarPlate { get; set; } = null!;
    public string CarModel { get; set; } = null!;
    public string CarBrand { get; set; } = null!;
    public string PickupLocation { get; set; } = null!;
    public string DropoffLocation { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public ReservationStatus Status { get; set; }
    public List<PaymentResponseDto> Payments { get; set; } = new();
}
