namespace RentACarProject.Application.DTOs.Location
{
    public class CreateLocationDto
    {
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Description { get; set; }
    }
}
