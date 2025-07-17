using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Domain.DTOs.Car;

public class GetAllCarsQuery : IRequest<ServiceResponse<List<CarResponseDto>>>
{
    public Guid? BrandId { get; set; }
    public Guid? ModelId { get; set; }
    public string? BrandName { get; set; }
    public string? ModelName { get; set; }
    public int? MinYear { get; set; }
    public int? MaxYear { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? Status { get; set; }
}
