using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACarProject.Application.Features.Reservation.Queries;
using RentACarProject.Application.Features.Reservation.Commands;

namespace RentACarProject.API.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [ApiExplorerSettings(GroupName = "Admin")]
    public class ReservationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReservationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllReservationsQuery());
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _mediator.Send(new GetReservationByIdQuery(id));
            return Ok(response);
        }

        [HttpGet("car/{carId}")]
        public async Task<IActionResult> GetByCarId(Guid carId)
        {
            var response = await _mediator.Send(new GetReservationsByCarIdQuery(carId));
            return Ok(response);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var response = await _mediator.Send(new GetReservationsByUserIdQuery(userId));
            return Ok(response);
        }

        [HttpGet("completed")]
        public async Task<IActionResult> GetCompleted()
        {
            var response = await _mediator.Send(new GetCompletedReservationsQuery());
            return Ok(response);
        }

        [HttpGet("by-dates")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var query = new GetReservationsByDateRangeQuery(startDate, endDate);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateReservationCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var response = await _mediator.Send(new CancelReservationCommand { Id = id });
            return Ok(response);
        }
    }
}
