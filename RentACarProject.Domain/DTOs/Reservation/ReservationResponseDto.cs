namespace RentACarProject.Application.DTOs.Reservation
{
    public class ReservationResponseDto
    {
        public Guid ReservationId { get; set; }
        public Guid CarId { get; set; }
        public string CarName { get; set; } = null!;
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = null!;
        public string? Note { get; set; }
    }
}
