using Application.Features.PasswordChange.Commands.InitiatePasswordChange;
using Application.Features.PasswordChange.Commands.ConfirmPasswordChange;
using Application.Features.PasswordChange.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RentACarProject.WebAPI.Controllers.Public
{
    [ApiExplorerSettings(GroupName = "Public")]
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordChangeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PasswordChangeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("initiate")]
        public async Task<IActionResult> Initiate([FromBody] InitiatePasswordChangeCommand command)
        {
            PasswordChangeResponseDto result = await _mediator.Send(command);

            if (result is null || !result.Success)
            {
                return BadRequest(new
                {
                    success = false,
                    message = result?.Message ?? "Şifre değişikliği başlatılamadı."
                });
            }

            return Ok(new
            {
                success = true,
                message = result.Message
            });
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm([FromBody] ConfirmPasswordChangeCommand command)
        {
            PasswordChangeResponseDto result = await _mediator.Send(command);

            if (result is null || !result.Success)
            {
                return BadRequest(new
                {
                    success = false,
                    message = result?.Message ?? "Şifre güncelleme işlemi başarısız oldu."
                });
            }

            return Ok(new
            {
                success = true,
                message = result.Message
            });
        }
    }
}
