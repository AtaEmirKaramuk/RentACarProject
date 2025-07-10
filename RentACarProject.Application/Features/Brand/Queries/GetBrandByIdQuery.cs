using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Brand;

namespace RentACarProject.Application.Features.Brand.Queries
{
    public class GetBrandByIdQuery : IRequest<ServiceResponse<BrandResponseDto>>
    {
        public Guid BrandId { get; set; }
    }
}
