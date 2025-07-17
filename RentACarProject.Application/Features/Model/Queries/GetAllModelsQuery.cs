using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Domain.DTOs.Model;

namespace RentACarProject.Application.Features.Model.Queries
{
    public class GetAllModelsQuery : IRequest<ServiceResponse<List<ModelResponseDto>>>
    {
    }
}
