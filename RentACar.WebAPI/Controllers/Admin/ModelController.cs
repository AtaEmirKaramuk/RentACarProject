using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACarProject.Application.Features.Model.Commands;
using RentACarProject.Application.Features.Model.Queries;
using RentACarProject.Application.Common;

namespace RentACarProject.WebAPI.Controllers.Admin
{
    [ApiExplorerSettings(GroupName = "Admin")]
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ModelController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ModelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllModelsQuery());
            return this.ToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetModelByIdQuery { ModelId = id });
            return this.ToActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateModelCommand command)
        {
            var result = await _mediator.Send(command);
            return this.ToActionResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateModelCommand command)
        {
            var result = await _mediator.Send(command);
            return this.ToActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteModelCommand { ModelId = id };
            var result = await _mediator.Send(command);
            return this.ToActionResult(result);
        }
    }
}
