using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Model;
using RentACarProject.Application.Exceptions;

namespace RentACarProject.Application.Features.Model.Commands
{
    public class UpdateModelCommandHandler : IRequestHandler<UpdateModelCommand, ServiceResponse<ModelResponseDto>>
    {
        private readonly IModelRepository _modelRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateModelCommandHandler(IModelRepository modelRepository, IBrandRepository brandRepository, IUnitOfWork unitOfWork)
        {
            _modelRepository = modelRepository;
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<ModelResponseDto>> Handle(UpdateModelCommand request, CancellationToken cancellationToken)
        {
            var model = await _modelRepository.GetAsync(m => m.ModelId == request.ModelId);
            if (model == null)
            {
                throw new BusinessException("Model bulunamadı.");
            }

            var brand = await _brandRepository.GetAsync(b => b.BrandId == request.BrandId);
            if (brand == null)
            {
                throw new BusinessException("Bağlı olduğu marka bulunamadı.");
            }

            var existingModel = await _modelRepository.GetAsync(m => m.BrandId == request.BrandId && m.Name == request.Name && m.ModelId != request.ModelId);
            if (existingModel != null)
            {
                throw new BusinessException("Bu marka altında aynı isimde bir model zaten mevcut.");
            }

            model.Name = request.Name;
            model.BrandId = request.BrandId;

            await _modelRepository.UpdateAsync(model);
            await _unitOfWork.SaveChangesAsync();

            var dto = new ModelResponseDto
            {
                Id = model.ModelId,
                Name = model.Name,
                BrandId = brand.BrandId,
                BrandName = brand.Name
            };

            return new ServiceResponse<ModelResponseDto>
            {
                Success = true,
                Message = $"Model \"{dto.Name}\" başarıyla güncellendi.",
                Data = dto
            };
        }
    }
}
