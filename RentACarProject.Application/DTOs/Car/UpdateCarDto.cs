namespace RentACarProject.Application.DTOs.Car
{
    public class UpdateCarDto
    {
        public Guid CarId { get; set; }
        public Guid ModelId { get; set; }
        public int Year { get; set; }
        public string Plate { get; set; } = null!;
        public decimal DailyPrice { get; set; }
        public bool Status { get; set; }
    }
}
