using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACarProject.Application.Features.Auth.Commands;
using RentACarProject.Application.Common;

namespace RentACarProject.WebAPI.Controllers.Admin
{
    [ApiExplorerSettings(GroupName = "Admin")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return this.ToActionResult(result);
        }
    }
}
