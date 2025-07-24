using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Exceptions;
using RentACarProject.Domain.Enums;

namespace RentACarProject.Application.Features.Payment.Commands
{
    public class ChangePaymentStatusCommandHandler : IRequestHandler<ChangePaymentStatusCommand, bool>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangePaymentStatusCommandHandler(IPaymentRepository paymentRepository, IUnitOfWork unitOfWork)
        {
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ChangePaymentStatusCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(request.PaymentId);
            if (payment == null || payment.IsDeleted)
                throw new NotFoundException("Ödeme kaydı bulunamadı.");

            payment.Status = request.NewStatus;
            await _paymentRepository.UpdateAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
