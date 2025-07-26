using FluentValidation;
using RentACarProject.Application.Features.Payment.Commands;

namespace RentACarProject.Application.Validators.Payment
{
    public class CancelPaymentValidator : AbstractValidator<CancelPaymentCommand>
    {
        public CancelPaymentValidator()
        {
            RuleFor(x => x.PaymentId)
                .NotEmpty().WithMessage("PaymentId boş olamaz.");
        }
    }
}
