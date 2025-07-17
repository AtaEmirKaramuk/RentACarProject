using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Domain.DTOs.Model;

public class DeleteModelCommand : IRequest<ServiceResponse<DeletedModelDto>>
{
    public Guid ModelId { get; set; }
}
