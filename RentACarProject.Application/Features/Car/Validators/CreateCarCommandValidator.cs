using FluentValidation;
using RentACarProject.Application.Features.Car.Commands;
using RentACarProject.Domain.Enums;

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

            RuleFor(x => x.VehicleClass)
                .IsInEnum().WithMessage("Geçersiz araç sınıfı seçildi.");

            RuleFor(x => x.FuelType)
                .IsInEnum().WithMessage("Geçersiz yakıt tipi seçildi.");

            RuleFor(x => x.TransmissionType)
                .IsInEnum().WithMessage("Geçersiz vites tipi seçildi.");
        }
    }
}
