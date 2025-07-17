namespace RentACarProject.Application.DTOs.Reservation
{
    public class CreateReservationDto
    {
        public Guid CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Note { get; set; }
    }
}
