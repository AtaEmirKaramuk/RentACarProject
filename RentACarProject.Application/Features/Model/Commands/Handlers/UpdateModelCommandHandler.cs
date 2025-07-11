using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Model;

namespace RentACarProject.Application.Features.Model.Commands.Handlers
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
                return new ServiceResponse<ModelResponseDto>
                {
                    Success = false,
                    Message = "Model bulunamadı.",
                    Code = "404"
                };
            }

            var brand = await _brandRepository.GetAsync(b => b.BrandId == request.BrandId);
            if (brand == null)
            {
                return new ServiceResponse<ModelResponseDto>
                {
                    Success = false,
                    Message = "Bağlı olduğu marka bulunamadı.",
                    Code = "404"
                };
            }

            var existingModel = await _modelRepository.GetAsync(m => m.BrandId == request.BrandId && m.Name == request.Name && m.ModelId != request.ModelId);
            if (existingModel != null)
            {
                return new ServiceResponse<ModelResponseDto>
                {
                    Success = false,
                    Message = "Bu marka altında aynı isimde bir model zaten mevcut.",
                    Code = "400"
                };
            }

            model.Name = request.Name;
            model.BrandId = request.BrandId;

            await _modelRepository.UpdateAsync(model);
            await _unitOfWork.SaveChangesAsync();

            var dto = new ModelResponseDto
            {
                ModelId = model.ModelId,
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
