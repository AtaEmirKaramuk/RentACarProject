using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Brand;
using RentACarProject.Domain.Entities;

namespace RentACarProject.Application.Features.Brand.Commands.Handlers
{
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, ServiceResponse<BrandResponseDto>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<BrandResponseDto>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            // Aynı isimde marka var mı kontrol
            var existingBrand = await _brandRepository.GetByNameAsync(request.Brand.Name);
            if (existingBrand != null)
            {
                return new ServiceResponse<BrandResponseDto>
                {
                    Success = false,
                    Message = "Bu marka ismi zaten mevcut.",
                    Code = "400"
                };
            }

            var newBrand = new RentACarProject.Domain.Entities.Brand
            {
                BrandId = Guid.NewGuid(),
                Name = request.Brand.Name
            };

            await _brandRepository.AddAsync(newBrand);
            await _unitOfWork.SaveChangesAsync();

            var dto = new BrandResponseDto
            {
                BrandId = newBrand.BrandId,
                Name = newBrand.Name
            };

            return new ServiceResponse<BrandResponseDto>
            {
                Success = true,
                Message = "Marka başarıyla oluşturuldu.",
                Data = dto
            };
        }
    }
}
