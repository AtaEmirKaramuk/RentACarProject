using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Domain.DTOs.Brand;
using RentACarProject.Application.Exceptions;

namespace RentACarProject.Application.Features.Brand.Commands;

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
        var brand = await _brandRepository.GetAsync(x => x.BrandId == request.Brand.Id);
        if (brand == null)
        {
            throw new BusinessException("Güncellenmek istenen marka bulunamadı.");
        }

        var duplicateBrand = await _brandRepository.GetByNameAsync(request.Brand.Name);
        if (duplicateBrand != null && duplicateBrand.BrandId != request.Brand.Id)
        {
            throw new BusinessException("Bu marka ismi başka bir marka tarafından kullanılıyor.");
        }

        brand.Name = request.Brand.Name;

        await _brandRepository.UpdateAsync(brand);
        await _unitOfWork.SaveChangesAsync();

        var dto = new BrandResponseDto
        {
            Id = brand.BrandId,
            Name = brand.Name
        };

        return new ServiceResponse<BrandResponseDto>
        {
            Success = true,
            Message = $"Marka \"{dto.Name}\" olarak güncellendi.",
            Data = dto
        };
    }
}
