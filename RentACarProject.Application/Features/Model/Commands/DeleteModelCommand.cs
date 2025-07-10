using MediatR;
using RentACarProject.Application.Common;
using System.Diagnostics;

namespace RentACarProject.Application.Features.Model.Commands
{
    public class DeleteModelCommand : IRequest<ServiceResponse<Guid>>
    {
        public Guid ModelId { get; set; }
    }
}
   
