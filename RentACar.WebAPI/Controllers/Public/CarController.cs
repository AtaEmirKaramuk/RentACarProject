using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACarProject.Application.Common;
using RentACarProject.Application.Features.Car.Queries;

namespace RentACarProject.WebAPI.Controllers.Public
{
    [ApiExplorerSettings(GroupName = "Public")]
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CarController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCarsQuery query)
        {
            var result = await _mediator.Send(query);
            return this.ToActionResult(result);
        }

        [AllowAnonymous]
        [HttpGet("by-brand/{brandName}")]
        public async Task<IActionResult> GetByBrand(string brandName)
        {
            var result = await _mediator.Send(new GetCarsByBrandQuery { BrandName = brandName });
            return this.ToActionResult(result);
        }

        [AllowAnonymous]
        [HttpGet("by-model/{modelName}")]
        public async Task<IActionResult> GetByModel(string modelName)
        {
            var result = await _mediator.Send(new GetCarsByModelQuery { ModelName = modelName });
            return this.ToActionResult(result);
        }
    }
}
