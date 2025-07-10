namespace RentACarProject.Application.DTOs.Brand
{
    public class UpdateBrandDto
    {
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
    }
}
