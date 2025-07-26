using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Model;
using RentACarProject.Application.Exceptions;

namespace RentACarProject.Application.Features.Model.Commands
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
            var brand = await _brandRepository.GetAsync(b => b.Id == request.BrandId);
            if (brand == null)
            {
                throw new BusinessException("Bağlı olduğu marka bulunamadı.");
            }

            var existingModel = await _modelRepository.GetAsync(m => m.BrandId == request.BrandId && m.Name == request.Name);
            if (existingModel != null)
            {
                throw new BusinessException("Bu marka altında aynı isimde bir model zaten mevcut.");
            }

            var newModel = new Domain.Entities.Model
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                BrandId = request.BrandId
            };

            await _modelRepository.AddAsync(newModel);
            await _unitOfWork.SaveChangesAsync();

            var dto = new ModelResponseDto
            {
                Id = newModel.Id,
                Name = newModel.Name,
                BrandId = brand.Id,
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
