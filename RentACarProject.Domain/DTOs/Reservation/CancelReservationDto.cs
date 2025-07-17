namespace RentACarProject.Application.DTOs.Reservation
{
    public class CancelReservationDto
    {
        public Guid ReservationId { get; set; }
        public string? Note { get; set; }
    }
}
