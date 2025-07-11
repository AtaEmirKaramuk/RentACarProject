using MediatR;
using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Model;

namespace RentACarProject.Application.Features.Model.Queries.Handlers
{
    public class GetAllModelsQueryHandler : IRequestHandler<GetAllModelsQuery, ServiceResponse<List<ModelResponseDto>>>
    {
        private readonly IModelRepository _modelRepository;

        public GetAllModelsQueryHandler(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public async Task<ServiceResponse<List<ModelResponseDto>>> Handle(GetAllModelsQuery request, CancellationToken cancellationToken)
        {
            var models = await _modelRepository.Query()
                .Include(m => m.Brand)
                .Select(m => new ModelResponseDto
                {
                    ModelId = m.ModelId,
                    Name = m.Name,
                    BrandId = m.BrandId,
                    BrandName = m.Brand.Name
                })
                .ToListAsync(cancellationToken);

            return new ServiceResponse<List<ModelResponseDto>>
            {
                Success = true,
                Message = "Modeller başarıyla listelendi.",
                Data = models
            };
        }
    }
}
