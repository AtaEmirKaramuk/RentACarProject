using MediatR;
using RentACarProject.Application.Common;

namespace RentACarProject.Application.Features.Model.Commands
{
    public class UpdateModelCommand : IRequest<ServiceResponse<Guid>>
    {
        public Guid ModelId { get; set; }
        public string Name { get; set; } = null!;
        public Guid BrandId { get; set; }
    }
}
