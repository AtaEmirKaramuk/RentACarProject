using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentACarProject.Application.DTOs.Location;
using RentACarProject.Application.Features.Location.Commands;
using RentACarProject.Application.Features.Location.Queries;

namespace RentACarProject.WebAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [ApiExplorerSettings(GroupName = "Admin")]
    public class LocationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LocationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLocationCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.LocationId }, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateLocationCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteLocationCommand(id));
            return result ? NoContent() : NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<List<LocationResponseDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllLocationsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LocationResponseDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetLocationByIdQuery(id));
            return Ok(result);
        }
    }
}
