using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Model;

namespace RentACarProject.Application.Features.Model.Commands.Handlers
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
                return new ServiceResponse<DeletedModelDto>
                {
                    Success = false,
                    Message = "Model bulunamadı.",
                    Code = "404"
                };
            }

            await _modelRepository.DeleteAsync(model);
            await _unitOfWork.SaveChangesAsync();

            var dto = new DeletedModelDto
            {
                ModelId = model.ModelId,
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
