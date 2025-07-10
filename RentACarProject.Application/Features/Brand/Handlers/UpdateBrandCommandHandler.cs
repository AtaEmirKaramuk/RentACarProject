using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Brand;

namespace RentACarProject.Application.Features.Brand.Commands.Handlers
{
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, ServiceResponse<BrandResponseDto>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<BrandResponseDto>> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            // 🔎 Mevcut marka var mı?
            var brand = await _brandRepository.GetAsync(x => x.BrandId == request.Brand.BrandId);
            if (brand == null)
            {
                return new ServiceResponse<BrandResponseDto>
                {
                    Success = false,
                    Message = "Güncellenmek istenen marka bulunamadı.",
                    Code = "404"
                };
            }

            // 🔎 Aynı isimde başka marka var mı kontrol
            var duplicateBrand = await _brandRepository.GetByNameAsync(request.Brand.Name);
            if (duplicateBrand != null && duplicateBrand.BrandId != request.Brand.BrandId)
            {
                return new ServiceResponse<BrandResponseDto>
                {
                    Success = false,
                    Message = "Bu marka ismi başka bir marka tarafından kullanılıyor.",
                    Code = "400"
                };
            }

            // 💡 Güncelleme
            brand.Name = request.Brand.Name;

            await _brandRepository.UpdateAsync(brand);
            await _unitOfWork.SaveChangesAsync();

            var dto = new BrandResponseDto
            {
                BrandId = brand.BrandId,
                Name = brand.Name
            };

            return new ServiceResponse<BrandResponseDto>
            {
                Success = true,
                Message = "Marka başarıyla güncellendi.",
                Data = dto
            };
        }
    }
}
