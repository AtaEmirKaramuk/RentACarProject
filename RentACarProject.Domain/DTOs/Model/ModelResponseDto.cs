namespace RentACarProject.Domain.DTOs.Model
{
    public class ModelResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid BrandId { get; set; }
        public string BrandName { get; set; } = null!;
    }
}
