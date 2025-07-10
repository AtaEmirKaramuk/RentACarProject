using MediatR;
using RentACarProject.Application.Common;

namespace RentACarProject.Application.Features.Car.Commands
{
    public class CreateCarCommand : IRequest<ServiceResponse<Guid>>
    {
        public Guid ModelId { get; set; }
        public int Year { get; set; }
        public string Plate { get; set; } = null!;
        public decimal DailyPrice { get; set; }
        public string? Description { get; set; }
    }
}
