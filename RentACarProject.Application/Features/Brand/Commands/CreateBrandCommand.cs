using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Brand;

namespace RentACarProject.Application.Features.Brand.Commands
{
    public class CreateBrandCommand : IRequest<ServiceResponse<BrandResponseDto>>
    {
        public CreateBrandDto Brand { get; set; } = null!;
    }
}
