namespace RentACarProject.Application.DTOs.Reservation
{
    public class UpdateReservationDto
    {
        public Guid ReservationId { get; set; }
        public Guid CarId { get; set; }
        public Guid PickupLocationId { get; set; }
        public Guid DropoffLocationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
