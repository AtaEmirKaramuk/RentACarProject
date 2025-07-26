using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Application.Features.Payment.Commands;
using RentACarProject.Application.Features.Payment.Queries;

[Route("api/payments")]
[ApiController]
[Authorize(Roles = "User")]
[ApiExplorerSettings(GroupName = "Public")]
public class PaymentController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ✅ 1.1. Ödeme Oluştur (Kredi Kartı)
    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] CreateCardPaymentDto dto)
    {
        var result = await _mediator.Send(new CreateCardPaymentCommand(dto));
        return Ok(result);
    }

    // ✅ 1.2. Ödeme Oluştur (Banka Havalesi)
    [HttpPost("bank-transfer")]
    public async Task<IActionResult> CreateBankTransferPayment([FromBody] CreateBankTransferDto dto)
    {
        var result = await _mediator.Send(new CreateBankTransferPaymentCommand(dto));
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

    // ✅ 4. Ödeme Güncelleme
    [HttpPut]
    public async Task<IActionResult> UpdatePayment([FromBody] UpdatePaymentDto dto)
    {
        var result = await _mediator.Send(new UpdatePaymentCommand(dto));
        return Ok(result);
    }

    // ✅ 5. Ödeme İptali
    [HttpDelete("{paymentId}")]
    public async Task<IActionResult> CancelPayment(Guid paymentId)
    {
        var result = await _mediator.Send(new CancelPaymentCommand(paymentId));
        return Ok(result);
    }
}
