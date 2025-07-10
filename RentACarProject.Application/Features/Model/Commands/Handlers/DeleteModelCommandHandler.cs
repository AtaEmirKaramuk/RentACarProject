using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;

namespace RentACarProject.Application.Features.Model.Commands.Handlers
{
    public class DeleteModelCommandHandler : IRequestHandler<DeleteModelCommand, ServiceResponse<Guid>>
    {
        private readonly IModelRepository _modelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteModelCommandHandler(IModelRepository modelRepository, IUnitOfWork unitOfWork)
        {
            _modelRepository = modelRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<Guid>> Handle(DeleteModelCommand request, CancellationToken cancellationToken)
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

            await _modelRepository.DeleteAsync(model);
            await _unitOfWork.SaveChangesAsync();

            return new ServiceResponse<Guid>
            {
                Success = true,
                Message = "Model başarıyla silindi.",
                Data = model.ModelId
            };
        }
    }
}
