using MediatR;
using RentACarProject.Application.Common;

namespace RentACarProject.Application.Features.Brand.Commands
{
    public class DeleteBrandCommand : IRequest<ServiceResponse<Guid>>
    {
        public Guid BrandId { get; set; }
    }
}
