public class UpdateCarDto
{
    public Guid Id { get; set; }              // ✅ CarId → Id
    public Guid ModelId { get; set; }
    public int Year { get; set; }
    public string Plate { get; set; } = null!;
    public decimal DailyPrice { get; set; }
    public bool Status { get; set; }
}
