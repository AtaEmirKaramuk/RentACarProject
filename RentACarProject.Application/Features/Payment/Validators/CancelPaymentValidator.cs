using FluentValidation;
using RentACarProject.Application.Features.Payment.Commands;

public class CancelPaymentCommandValidator : AbstractValidator<CancelPaymentCommand>
{
    public CancelPaymentCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty().WithMessage("Ödeme ID boş olamaz.");
    }
}
