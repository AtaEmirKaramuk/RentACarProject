namespace RentACarProject.Application.DTOs.Car
{
    public class CarResponseDto
    {
        public Guid CarId { get; set; }
        public string BrandName { get; set; } = null!;
        public string ModelName { get; set; } = null!;
        public int Year { get; set; }
        public string Plate { get; set; } = null!;
        public decimal DailyPrice { get; set; }
        public bool Status { get; set; }
    }
}
