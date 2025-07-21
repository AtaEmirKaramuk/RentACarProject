using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Reservation;
using RentACarProject.Application.Features.Reservation.Commands;
using RentACarProject.Application.Features.Reservation.Queries;

namespace RentACarProject.API.Controllers.Public
{
    [Route("api/public/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public ReservationController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<ReservationResponseDto>>> Create(CreateReservationDto dto)
        {
            var command = new CreateReservationCommand { Reservation = dto };
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("my")]
        public async Task<ActionResult<ServiceResponse<List<ReservationResponseDto>>>> GetMyReservations()
        {
            if (!_currentUserService.UserId.HasValue)
                return Unauthorized(new ServiceResponse<string> { Success = false, Message = "Kullanıcı bilgisi alınamadı." });

            var query = new GetReservationsByUserIdQuery { UserId = _currentUserService.UserId.Value };
            return Ok(await _mediator.Send(query));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<ReservationResponseDto>>> Update(UpdateReservationDto dto)
        {
            var command = new UpdateReservationCommand { Reservation = dto };
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ServiceResponse<string>>> Cancel(Guid id)
        {
            var command = new CancelReservationCommand { Id = id };
            return Ok(await _mediator.Send(command));
        }
    }
}
