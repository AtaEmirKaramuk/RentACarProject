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
                .NotEmpty().WithMessage("Kart sahibi adı girilmelidir.")
                .MaximumLength(100).WithMessage("Kart sahibi adı en fazla 100 karakter olmalıdır.");

            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Kart numarası girilmelidir.")
                .Matches(@"^\d{16}$").WithMessage("Kart numarası 16 haneli olmalıdır.");

            RuleFor(x => x.ExpireMonth)
                .InclusiveBetween(1, 12).WithMessage("Son kullanma ayı 1 ile 12 arasında olmalıdır.");

            RuleFor(x => x.ExpireYear)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("Geçerli bir yıl girilmelidir.");

            RuleFor(x => x.Cvc)
                .NotEmpty().WithMessage("CVC kodu girilmelidir.")
                .Length(3).WithMessage("CVC kodu 3 haneli olmalıdır.");

            RuleFor(x => x.InstallmentCount)
                .InclusiveBetween(1, 12).When(x => x.InstallmentCount.HasValue)
                .WithMessage("Taksit sayısı 1 ile 12 arasında olmalıdır.");
        }
    }
}
