using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Domain.DTOs.Brand;

namespace RentACarProject.Application.Features.Brand.Commands
{
    public class CreateBrandCommand : IRequest<ServiceResponse<BrandResponseDto>>
    {
        public CreateBrandDto Brand { get; set; } = null!;
    }
}
