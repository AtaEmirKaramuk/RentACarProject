namespace RentACarProject.Application.DTOs.Car
{
    public class CreateCarDto
    {
        public Guid ModelId { get; set; }
        public int Year { get; set; }
        public string Plate { get; set; } = null!;
        public decimal DailyPrice { get; set; }
    }
}
