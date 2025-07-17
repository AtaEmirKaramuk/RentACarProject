using MediatR;
using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Domain.DTOs.Model;

namespace RentACarProject.Application.Features.Model.Queries
{
    public class GetModelByIdQueryHandler : IRequestHandler<GetModelByIdQuery, ServiceResponse<ModelResponseDto>>
    {
        private readonly IModelRepository _modelRepository;

        public GetModelByIdQueryHandler(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public async Task<ServiceResponse<ModelResponseDto>> Handle(GetModelByIdQuery request, CancellationToken cancellationToken)
        {
            var model = await _modelRepository.Query()
                .Include(m => m.Brand)
                .Where(m => m.ModelId == request.ModelId)
                .Select(m => new ModelResponseDto
                {
                    Id = m.ModelId,
                    Name = m.Name,
                    BrandId = m.BrandId,
                    BrandName = m.Brand.Name
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (model == null)
            {
                return new ServiceResponse<ModelResponseDto>
                {
                    Success = false,
                    Message = "Model bulunamadı.",
                    Code = "404"
                };
            }

            return new ServiceResponse<ModelResponseDto>
            {
                Success = true,
                Message = $"Model \"{model.Name}\" başarıyla getirildi.",
                Data = model
            };
        }
    }
}
