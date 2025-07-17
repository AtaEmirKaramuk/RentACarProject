using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Domain.DTOs.Car;

namespace RentACarProject.Application.Features.Car.Commands
{
    public class UpdateCarCommand : IRequest<ServiceResponse<CarResponseDto>>
    {
        public Guid CarId { get; set; }
        public Guid ModelId { get; set; }
        public int Year { get; set; }
        public string Plate { get; set; } = null!;
        public decimal DailyPrice { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}
