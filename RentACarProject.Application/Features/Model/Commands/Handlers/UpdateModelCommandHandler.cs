using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;

namespace RentACarProject.Application.Features.Model.Commands.Handlers
{
    public class UpdateModelCommandHandler : IRequestHandler<UpdateModelCommand, ServiceResponse<Guid>>
    {
        private readonly IModelRepository _modelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateModelCommandHandler(IModelRepository modelRepository, IUnitOfWork unitOfWork)
        {
            _modelRepository = modelRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<Guid>> Handle(UpdateModelCommand request, CancellationToken cancellationToken)
        {
            var model = await _modelRepository.GetAsync(x => x.ModelId == request.ModelId);
            if (model == null)
            {
                return new ServiceResponse<Guid>
                {
                    Success = false,
                    Message = "Model bulunamadı.",
                    Code = "404"
                };
            }

            model.Name = request.Name;
            model.BrandId = request.BrandId;

            await _modelRepository.UpdateAsync(model);
            await _unitOfWork.SaveChangesAsync();

            return new ServiceResponse<Guid>
            {
                Success = true,
                Message = "Model başarıyla güncellendi.",
                Data = model.ModelId
            };
        }
    }
}
