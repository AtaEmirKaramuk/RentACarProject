using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACarProject.Application.Features.Car.Commands;
using RentACarProject.Application.Features.Car.Queries;
using RentACarProject.Application.Common;
using RentACarProject.Domain.Enums;

namespace RentACarProject.WebAPI.Controllers.Admin
{
    [ApiExplorerSettings(GroupName = "Admin")]
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CarController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CarController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCarsQuery query)
        {
            var result = await _mediator.Send(query);
            return this.ToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetCarByIdQuery { CarId = id });
            return this.ToActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCarCommand command)
        {
            var result = await _mediator.Send(command);
            return this.ToActionResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCarCommand command)
        {
            var result = await _mediator.Send(command);
            return this.ToActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteCarCommand { Id = id };
            var result = await _mediator.Send(command);
            return this.ToActionResult(result);
        }
    }
}
