using FluentValidation;
using RentACarProject.Application.Features.Brand.Commands;

namespace RentACarProject.Application.Validators.Brand
{
    public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
    {
        public CreateBrandCommandValidator()
        {
            RuleFor(x => x.Brand.Name)
                .NotEmpty().WithMessage("Marka adı zorunludur.")
                .MinimumLength(2).WithMessage("Marka adı en az 2 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Marka adı en fazla 50 karakter olabilir.")
                .Matches(@"^[a-zA-Z0-9çÇğĞıİöÖşŞüÜ\s]+$")
                .WithMessage("Marka adı sadece harf, rakam ve boşluk içerebilir.");
        }
    }
}
