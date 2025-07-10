namespace RentACarProject.Application.DTOs.Model
{
    public class ModelResponseDto
    {
        public Guid ModelId { get; set; }
        public string Name { get; set; } = null!;
        public string BrandName { get; set; } = null!;
    }
}
