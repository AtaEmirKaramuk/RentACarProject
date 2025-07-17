using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Domain.DTOs.Brand;

namespace RentACarProject.Application.Features.Brand.Commands
{
    public class DeleteBrandCommand : IRequest<ServiceResponse<DeletedBrandDto>>
    {
        public Guid BrandId { get; set; }
    }
}
