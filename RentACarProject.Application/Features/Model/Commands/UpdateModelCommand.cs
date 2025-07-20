using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Model;

public class UpdateModelCommand : IRequest<ServiceResponse<ModelResponseDto>>
{
    public Guid ModelId { get; set; }
    public string Name { get; set; } = null!;
    public Guid BrandId { get; set; }
}
