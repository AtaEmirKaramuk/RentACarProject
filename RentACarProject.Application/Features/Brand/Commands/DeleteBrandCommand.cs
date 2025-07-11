using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Brand;

namespace RentACarProject.Application.Features.Brand.Commands
{
    public class DeleteBrandCommand : IRequest<ServiceResponse<DeletedBrandDto>>
    {
        public Guid BrandId { get; set; }
    }
}
