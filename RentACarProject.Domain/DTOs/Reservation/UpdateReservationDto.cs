namespace RentACarProject.Application.DTOs.Reservation
{
    public class UpdateReservationDto
    {
        public Guid ReservationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Note { get; set; }
    }
}
