using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Application.Features.Payment.Commands;
using RentACarProject.Application.Features.Payment.Queries;

namespace RentACarProject.API.Controllers.Public
{
    [Route("api/payments")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ✅ 1. Ödeme Oluştur (Havale veya Kredi Kartı)
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto dto)
        {
            var result = await _mediator.Send(new CreatePaymentCommand(dto));
            return Ok(result);
        }

        // ✅ 2. Kullanıcının Ödeme Listesi
        [HttpGet("me")]
        public async Task<IActionResult> GetMyPayments([FromQuery] GetMyPaymentsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // ✅ 3. Kullanıcının Belirli Ödeme Detayı
        [HttpGet("me/{paymentId}")]
        public async Task<IActionResult> GetMyPaymentDetail(Guid paymentId)
        {
            var result = await _mediator.Send(new GetMyPaymentDetailQuery(paymentId));
            return Ok(result);
        }
    }
}
