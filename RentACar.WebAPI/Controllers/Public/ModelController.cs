using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACarProject.Application.Features.Model.Queries;
using RentACarProject.Application.Common;

namespace RentACarProject.WebAPI.Controllers.Public
{
    [ApiExplorerSettings(GroupName = "Public")]
    [ApiController]
    [Route("api/[controller]")]
    public class ModelController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ModelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllModelsQuery());
            return this.ToActionResult(result);
        }
    }
}
