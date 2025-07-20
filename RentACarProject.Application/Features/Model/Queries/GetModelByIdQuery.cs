using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Model;

namespace RentACarProject.Application.Features.Model.Queries
{
    public class GetModelByIdQuery : IRequest<ServiceResponse<ModelResponseDto>>
    {
        public Guid ModelId { get; set; }
    }
}
