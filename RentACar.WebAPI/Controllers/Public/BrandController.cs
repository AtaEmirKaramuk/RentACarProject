using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACarProject.Application.Features.Brand.Queries;
using RentACarProject.Application.Common;

namespace RentACarProject.WebAPI.Controllers.Public
{
    [ApiExplorerSettings(GroupName = "Public")]
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllBrandsQuery());
            return this.ToActionResult(result);
        }
    }
}
