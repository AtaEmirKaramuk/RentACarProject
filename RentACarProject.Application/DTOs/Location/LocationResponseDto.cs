namespace RentACarProject.Application.DTOs.Location
{
    public class LocationResponseDto
    {
        public Guid LocationId { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Description { get; set; }
    }
}
