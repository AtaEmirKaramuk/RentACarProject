namespace RentACarProject.Application.DTOs.Payment
{
    public class BankTransferApprovalDto
    {
        public Guid PaymentId { get; set; }
        public string TransactionId { get; set; } = null!;
    }
}
