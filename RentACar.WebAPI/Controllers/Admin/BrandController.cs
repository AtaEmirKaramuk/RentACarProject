using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACarProject.Application.Features.Brand.Commands;
using RentACarProject.Application.Features.Brand.Queries;
using RentACarProject.Application.Common;

namespace RentACarProject.WebAPI.Controllers.Admin
{
    [ApiExplorerSettings(GroupName = "Admin")]
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class BrandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllBrandsQuery());
            return this.ToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetBrandByIdQuery { BrandId = id });
            return this.ToActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBrandCommand command)
        {
            var result = await _mediator.Send(command);
            return this.ToActionResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBrandCommand command)
        {
            var result = await _mediator.Send(command);
            return this.ToActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteBrandCommand { BrandId = id };
            var result = await _mediator.Send(command);
            return this.ToActionResult(result);
        }
    }
}
