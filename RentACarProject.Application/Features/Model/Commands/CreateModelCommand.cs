using MediatR;
using RentACarProject.Application.Common;

namespace RentACarProject.Application.Features.Model.Commands
{
    public class CreateModelCommand : IRequest<ServiceResponse<Guid>>
    {
        public string Name { get; set; } = null!;
        public Guid BrandId { get; set; }
    }
}
