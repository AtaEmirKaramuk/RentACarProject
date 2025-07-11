namespace RentACarProject.Application.DTOs.Model
{
    public class DeletedModelDto
    {
        public Guid ModelId { get; set; }
        public string Name { get; set; } = null!;
        public Guid BrandId { get; set; }
    }
}
