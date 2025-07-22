namespace RentACarProject.Application.DTOs.Location
{
    public class UpdateLocationDto
    {
        public Guid LocationId { get; set; }  // ID zorunlu
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Description { get; set; }
    }
}
