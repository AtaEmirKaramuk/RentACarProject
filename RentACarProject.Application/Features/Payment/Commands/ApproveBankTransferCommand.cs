using MediatR;
using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class ApproveBankTransferCommand : IRequest<PaymentResponseDto>
    {
        public BankTransferApprovalDto Approval { get; set; }

        public ApproveBankTransferCommand(BankTransferApprovalDto approval)
        {
            Approval = approval;
        }
    }
}
