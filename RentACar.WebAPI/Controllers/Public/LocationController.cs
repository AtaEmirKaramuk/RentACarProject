using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentACarProject.Application.Features.Location.Queries;
using RentACarProject.Application.DTOs.Location;

namespace RentACarProject.WebAPI.Controllers.Public
{
    [ApiController]
    [Route("api/public/[controller]")]
    [ApiExplorerSettings(GroupName = "Public")]
    public class LocationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LocationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<LocationResponseDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllLocationsQuery());
            return Ok(result);
        }

        
    }
}
