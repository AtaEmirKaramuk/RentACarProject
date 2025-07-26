using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Brand;
using RentACarProject.Application.Exceptions;

namespace RentACarProject.Application.Features.Brand.Commands
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
            var existingBrand = await _brandRepository.GetByNameAsync(request.Brand.Name);
            if (existingBrand != null)
            {
                throw new BusinessException("Bu marka ismi zaten mevcut.");
            }

            var newBrand = new Domain.Entities.Brand
            {
                Id = Guid.NewGuid(),
                Name = request.Brand.Name
            };

            await _brandRepository.AddAsync(newBrand);
            await _unitOfWork.SaveChangesAsync();

            var dto = new BrandResponseDto
            {
                Id = newBrand.Id,
                Name = newBrand.Name
            };

            return new ServiceResponse<BrandResponseDto>
            {
                Success = true,
                Message = $"Marka \"{dto.Name}\" başarıyla oluşturuldu.",
                Data = dto
            };
        }
    }
}
