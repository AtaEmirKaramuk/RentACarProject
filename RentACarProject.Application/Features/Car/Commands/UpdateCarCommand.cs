using MediatR;
using RentACarProject.Application.Common;

namespace RentACarProject.Application.Features.Car.Commands
{
    public class UpdateCarCommand : IRequest<ServiceResponse<Guid>>
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
