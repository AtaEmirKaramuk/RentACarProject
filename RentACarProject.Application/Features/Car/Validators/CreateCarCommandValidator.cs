using FluentValidation;
using RentACarProject.Application.Features.Car.Commands;

namespace RentACarProject.Application.Features.Car.Validators
{
    public class CreateCarCommandValidator : AbstractValidator<CreateCarCommand>
    {
        public CreateCarCommandValidator()
        {
            RuleFor(x => x.ModelId)
                .NotEmpty().WithMessage("Model seçilmelidir.");

            RuleFor(x => x.Year)
                .InclusiveBetween(1990, DateTime.UtcNow.Year + 1)
                .WithMessage("Geçerli bir üretim yılı giriniz.");

            RuleFor(x => x.Plate)
                .NotEmpty().WithMessage("Plaka zorunludur.")
                .MaximumLength(20).WithMessage("Plaka en fazla 20 karakter olabilir.");

            RuleFor(x => x.DailyPrice)
                .GreaterThan(0).WithMessage("Günlük fiyat 0'dan büyük olmalıdır.")
                .LessThan(50000).WithMessage("Günlük fiyat makul bir değerden yüksek olamaz.");

            // opsiyonel: açıklama boş olabilir, o yüzden validator zorunlu değil
        }
    }
}
