using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Domain.Entities;

namespace RentACarProject.Application.Features.Model.Commands.Handlers
{
    public class CreateModelCommandHandler : IRequestHandler<CreateModelCommand, ServiceResponse<Guid>>
    {
        private readonly IModelRepository _modelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateModelCommandHandler(IModelRepository modelRepository, IUnitOfWork unitOfWork)
        {
            _modelRepository = modelRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<Guid>> Handle(CreateModelCommand request, CancellationToken cancellationToken)
        {
            var newModel = new Domain.Entities.Model
            {
                ModelId = Guid.NewGuid(),
                Name = request.Name,
                BrandId = request.BrandId
            };

            await _modelRepository.AddAsync(newModel);
            await _unitOfWork.SaveChangesAsync();

            return new ServiceResponse<Guid>
            {
                Success = true,
                Message = "Model başarıyla eklendi.",
                Data = newModel.ModelId
            };
        }
    }
}
