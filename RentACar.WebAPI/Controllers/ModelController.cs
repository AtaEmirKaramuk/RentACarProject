using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentACarProject.Application.Features.Model.Commands;
using RentACarProject.Application.Features.Model.Queries;

namespace RentACarProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ModelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //  Tüm modelleri listele
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllModelsQuery());
            return Ok(result);
        }

        //  Model detay (Id'ye göre)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetModelByIdQuery { ModelId = id });
            return Ok(result);
        }

        //  Model ekle
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateModelCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        //  Model güncelle
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateModelCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        //  Model sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteModelCommand { ModelId = id });
            return Ok(result);
        }
    }
}
