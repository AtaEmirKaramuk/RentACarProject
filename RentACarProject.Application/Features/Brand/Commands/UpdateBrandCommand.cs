using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Domain.DTOs.Brand;

namespace RentACarProject.Application.Features.Brand.Commands
{
    public class UpdateBrandCommand : IRequest<ServiceResponse<BrandResponseDto>>
    {
        public UpdateBrandDto Brand { get; set; } = null!;
    }
}
