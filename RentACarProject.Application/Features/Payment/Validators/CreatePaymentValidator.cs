using FluentValidation;
using RentACarProject.Application.DTOs.Payment;

namespace RentACarProject.Application.Validators.Payment
{
    public class CreatePaymentValidator : AbstractValidator<CreateCardPaymentDto>
    {
        public CreatePaymentValidator()
        {
            RuleFor(x => x.ReservationId)
                .NotEmpty().WithMessage("Rezervasyon ID boş olamaz.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Ödeme tutarı sıfırdan büyük olmalıdır.");

            RuleFor(x => x.CardHolderName)
                .NotEmpty().WithMessage("Kart sahibi adı zorunludur.");

            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Kart numarası zorunludur.")
                .CreditCard().WithMessage("Geçersiz kart numarası.");

            RuleFor(x => x.ExpireMonth)
                .NotEmpty().WithMessage("Son kullanma ayı zorunludur.");

            RuleFor(x => x.ExpireYear)
                .NotEmpty().WithMessage("Son kullanma yılı zorunludur.");

            RuleFor(x => x.Cvc)
                .NotEmpty().WithMessage("CVC zorunludur.")
                .Length(3).WithMessage("CVC 3 haneli olmalıdır.");
        }
    }
}
