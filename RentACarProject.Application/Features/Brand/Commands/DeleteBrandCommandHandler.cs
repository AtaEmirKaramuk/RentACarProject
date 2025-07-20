using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Brand;
using RentACarProject.Application.Exceptions;

namespace RentACarProject.Application.Features.Brand.Commands
{
    public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, ServiceResponse<DeletedBrandDto>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<DeletedBrandDto>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetAsync(x => x.BrandId == request.BrandId);

            if (brand == null)
            {
                throw new BusinessException("Marka bulunamadı.");
            }

            await _brandRepository.DeleteAsync(brand);
            await _unitOfWork.SaveChangesAsync();

            var dto = new DeletedBrandDto
            {
                Id = brand.BrandId,
                Name = brand.Name
            };

            return new ServiceResponse<DeletedBrandDto>
            {
                Success = true,
                Message = $"Marka \"{dto.Name}\" başarıyla silindi.",
                Data = dto
            };
        }
    }
}
