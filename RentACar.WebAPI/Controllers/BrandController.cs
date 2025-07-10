using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACarProject.Application.Features.Brand.Commands;
using RentACarProject.Application.Features.Brand.Queries;

namespace RentACarProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //  Tüm markaları listele
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllBrandsQuery());
            return Ok(result);
        }

        //  Id'ye göre marka getir
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetBrandByIdQuery { BrandId = id });
            return Ok(result);
        }

        //  Marka oluştur
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBrandCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        //  Marka güncelle
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBrandCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        //  Marka sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteBrandCommand { BrandId = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
