using MediatR;
using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Features.Payment.Queries
{
    public class GetPendingBankTransferPaymentsQuery : IRequest<List<PaymentResponseDto>>
    {
    }
}
