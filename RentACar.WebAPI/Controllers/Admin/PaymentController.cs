using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACarProject.Application.Features.Payment.Commands;
using RentACarProject.Application.Features.Payment.Queries;
using RentACarProject.Domain.Enums;

namespace RentACarProject.WebAPI.Controllers.Admin
{
    [Route("api/admin/payments")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [ApiExplorerSettings(GroupName = "Admin")]
    public class AdminPaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminPaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 1. Tüm ödemeleri filtreyle getir
        [HttpGet]
        public async Task<IActionResult> GetAllPayments(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] PaymentStatus? status,
            [FromQuery] PaymentType? type,
            [FromQuery] Guid? reservationId,
            [FromQuery] Guid? userId)
        {
            var query = new GetAllPaymentsWithFiltersQuery(startDate, endDate, status, type, reservationId, userId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // 2. Bekleyen banka havalelerini getir
        [HttpGet("pending-bank-transfers")]
        public async Task<IActionResult> GetPendingBankTransfers()
        {
            var result = await _mediator.Send(new GetPendingBankTransferPaymentsQuery());
            return Ok(result);
        }

        // 3. Havale onayla (Yeni versiyon - dto kullanılmıyor)
        [HttpPost("bank-transfer/approve")]
        public async Task<IActionResult> ApproveBankTransfer(
            [FromQuery] Guid paymentId,
            [FromQuery] string transactionId)
        {
            var result = await _mediator.Send(new ApproveBankTransferCommand(paymentId, transactionId));
            return Ok(result);
        }

        // 4. Ödeme durumunu değiştir
        [HttpPatch("change-status")]
        public async Task<IActionResult> ChangePaymentStatus(
            [FromQuery] Guid paymentId,
            [FromQuery] PaymentStatus newStatus)
        {
            var command = new ChangePaymentStatusCommand(paymentId, newStatus);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
