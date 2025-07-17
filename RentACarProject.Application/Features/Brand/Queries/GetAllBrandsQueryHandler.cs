using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Domain.DTOs.Brand;

namespace RentACarProject.Application.Features.Brand.Queries
{
    public class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, ServiceResponse<List<BrandResponseDto>>>
    {
        private readonly IBrandRepository _brandRepository;

        public GetAllBrandsQueryHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<ServiceResponse<List<BrandResponseDto>>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _brandRepository.GetListAsync();

            var brandDtos = brands.Select(b => new BrandResponseDto
            {
                Id = b.BrandId,
                Name = b.Name
            }).ToList();

            return new ServiceResponse<List<BrandResponseDto>>
            {
                Success = true,
                Message = "Markalar başarıyla listelendi.",
                Data = brandDtos
            };
        }
    }
}
