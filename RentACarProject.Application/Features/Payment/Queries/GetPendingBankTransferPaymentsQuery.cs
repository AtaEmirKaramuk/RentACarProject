using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Features.Payment.Queries
{
    public class GetPendingBankTransferPaymentsQuery : IRequest<ServiceResponse<List<PaymentResponseDto>>>
    {
    }
}
