namespace RentACarProject.Application.DTOs.Model
{
    public class UpdateModelDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid BrandId { get; set; }
    }
}
