using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Model;

namespace RentACarProject.Application.Features.Model.Queries
{
    public class GetAllModelsQuery : IRequest<ServiceResponse<List<ModelResponseDto>>>
    {
    }
}
