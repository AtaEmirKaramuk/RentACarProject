using MediatR;
using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Domain.Enums;

namespace RentACarProject.Application.Features.Payment.Queries
{
    public class GetAllPaymentsWithFiltersQuery : IRequest<List<PaymentResponseDto>>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PaymentStatus? Status { get; set; }
        public PaymentType? Type { get; set; }
        public Guid? ReservationId { get; set; }
        public Guid? UserId { get; set; }

        public GetAllPaymentsWithFiltersQuery(DateTime? startDate = null, DateTime? endDate = null,
                                              PaymentStatus? status = null, PaymentType? type = null,
                                              Guid? reservationId = null, Guid? userId = null)
        {
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            Type = type;
            ReservationId = reservationId;
            UserId = userId;
        }
    }
}
