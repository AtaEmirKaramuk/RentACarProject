using FluentValidation;
using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Validators.Payment
{
    public class UpdatePaymentValidator : AbstractValidator<UpdatePaymentDto>
    {
        public UpdatePaymentValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Ödeme ID boş olamaz.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Geçersiz ödeme durumu.");

            When(x => !string.IsNullOrWhiteSpace(x.TransactionId), () =>
            {
                RuleFor(x => x.TransactionId)
                    .MaximumLength(100).WithMessage("İşlem numarası 100 karakteri geçemez.");
            });
        }
    }
}
