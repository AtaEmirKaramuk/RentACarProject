using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Model;

public class CreateModelCommand : IRequest<ServiceResponse<ModelResponseDto>>
{
    public string Name { get; set; } = null!;
    public Guid BrandId { get; set; }
}
