using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Model;
using RentACarProject.Application.Exceptions;

namespace RentACarProject.Application.Features.Model.Commands
{
    public class DeleteModelCommandHandler : IRequestHandler<DeleteModelCommand, ServiceResponse<DeletedModelDto>>
    {
        private readonly IModelRepository _modelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteModelCommandHandler(IModelRepository modelRepository, IUnitOfWork unitOfWork)
        {
            _modelRepository = modelRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<DeletedModelDto>> Handle(DeleteModelCommand request, CancellationToken cancellationToken)
        {
            var model = await _modelRepository.GetAsync(m => m.ModelId == request.ModelId);
            if (model == null)
            {
                throw new BusinessException("Model bulunamadı.");
            }

            await _modelRepository.DeleteAsync(model);
            await _unitOfWork.SaveChangesAsync();

            var dto = new DeletedModelDto
            {
                Id = model.ModelId,
                Name = model.Name,
                BrandId = model.BrandId
            };

            return new ServiceResponse<DeletedModelDto>
            {
                Success = true,
                Message = $"Model \"{dto.Name}\" başarıyla silindi.",
                Data = dto
            };
        }
    }
}
