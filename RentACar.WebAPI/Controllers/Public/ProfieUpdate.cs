using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.ProfileUpdate;
using RentACarProject.Application.Features.ProfileUpdate.Commands;
using RentACarProject.Application.Features.ProfileUpdate.Queries;

namespace RentACarProject.API.Controllers.Public
{
    [Route("api/public/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Kullanıcının kendi profil bilgilerini getirir.
        /// </summary>
        [HttpGet("me")]
        public async Task<ActionResult<ServiceResponse<UpdateProfileDto>>> GetMyProfile()
        {
            var response = await _mediator.Send(new GetMyProfileQuery());
            return Ok(response);
        }

        /// <summary>
        /// Kullanıcı profil bilgilerini günceller (partial update).
        /// </summary>
        [HttpPatch]
        public async Task<ActionResult<ServiceResponse<UpdateProfileDto>>> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var command = new UpdateProfileCommand { Profile = dto };
            var response = await _mediator.Send(command);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
