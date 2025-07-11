using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Model;

namespace RentACarProject.Application.Features.Model.Commands.Handlers
{
    public class CreateModelCommandHandler : IRequestHandler<CreateModelCommand, ServiceResponse<ModelResponseDto>>
    {
        private readonly IModelRepository _modelRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateModelCommandHandler(IModelRepository modelRepository, IBrandRepository brandRepository, IUnitOfWork unitOfWork)
        {
            _modelRepository = modelRepository;
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<ModelResponseDto>> Handle(CreateModelCommand request, CancellationToken cancellationToken)
        {
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

            var existingModel = await _modelRepository.GetAsync(m => m.BrandId == request.BrandId && m.Name == request.Name);
            if (existingModel != null)
            {
                return new ServiceResponse<ModelResponseDto>
                {
                    Success = false,
                    Message = "Bu marka altında aynı isimde bir model zaten mevcut.",
                    Code = "400"
                };
            }

            var newModel = new Domain.Entities.Model
            {
                ModelId = Guid.NewGuid(),
                Name = request.Name,
                BrandId = request.BrandId
            };

            await _modelRepository.AddAsync(newModel);
            await _unitOfWork.SaveChangesAsync();

            var dto = new ModelResponseDto
            {
                ModelId = newModel.ModelId,
                Name = newModel.Name,
                BrandId = brand.BrandId,
                BrandName = brand.Name
            };

            return new ServiceResponse<ModelResponseDto>
            {
                Success = true,
                Message = $"Model \"{dto.Name}\" başarıyla eklendi.",
                Data = dto
            };
        }
    }
}
