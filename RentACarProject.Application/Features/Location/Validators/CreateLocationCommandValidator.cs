using FluentValidation;
using RentACarProject.Application.Features.Location.Commands;

namespace RentACarProject.Application.Features.Location.Validators
{
    public class CreateLocationCommandValidator : AbstractValidator<CreateLocationCommand>
    {
        public CreateLocationCommandValidator()
        {
            RuleFor(x => x.Location.Name)
                .NotEmpty().WithMessage("Lokasyon adı boş olamaz.")
                .MaximumLength(100).WithMessage("Lokasyon adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Location.City)
                .NotEmpty().WithMessage("Şehir bilgisi boş olamaz.")
                .MaximumLength(50).WithMessage("Şehir en fazla 50 karakter olabilir.");

            RuleFor(x => x.Location.Address)
                .NotEmpty().WithMessage("Adres bilgisi boş olamaz.")
                .MaximumLength(250).WithMessage("Adres en fazla 250 karakter olabilir.");

            RuleFor(x => x.Location.Description)
                .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");
        }
    }
}
