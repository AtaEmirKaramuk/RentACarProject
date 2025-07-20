using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Brand;

namespace RentACarProject.Application.Features.Brand.Queries
{
    public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, ServiceResponse<BrandResponseDto>>
    {
        private readonly IBrandRepository _brandRepository;

        public GetBrandByIdQueryHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<ServiceResponse<BrandResponseDto>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetAsync(b => b.BrandId == request.BrandId);

            if (brand == null)
            {
                return new ServiceResponse<BrandResponseDto>
                {
                    Success = false,
                    Message = "Marka bulunamadı.",
                    Code = "404"
                };
            }

            var dto = new BrandResponseDto
            {
                Id = brand.BrandId,
                Name = brand.Name
            };

            return new ServiceResponse<BrandResponseDto>
            {
                Success = true,
                Message = $"Marka \"{dto.Name}\" başarıyla getirildi.",
                Data = dto
            };
        }
    }
}
