public class DeletedCarDto
{
    public Guid Id { get; set; }                
    public string Plate { get; set; } = null!;
    public string ModelName { get; set; } = null!;
    public string BrandName { get; set; } = null!;
}
