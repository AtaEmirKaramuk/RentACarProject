using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Domain.DTOs.Brand;

namespace RentACarProject.Application.Features.Brand.Queries
{
    public class GetAllBrandsQuery : IRequest<ServiceResponse<List<BrandResponseDto>>>
    {
    }
}
