using MediatR;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Domain.Enums;

namespace RentACarProject.Application.Features.Payment.Queries
{
    public class GetMyPaymentsQuery : IRequest<ServiceResponse<List<PaymentResponseDto>>>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PaymentStatus? Status { get; set; }
        public PaymentType? Type { get; set; }

        public GetMyPaymentsQuery(DateTime? startDate = null, DateTime? endDate = null,
                                  PaymentStatus? status = null, PaymentType? type = null)
        {
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            Type = type;
        }
    }
}
