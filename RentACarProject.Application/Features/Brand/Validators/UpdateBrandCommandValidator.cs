using FluentValidation;
using RentACarProject.Application.Features.Brand.Commands;

public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
{
    public UpdateBrandCommandValidator()
    {
        RuleFor(x => x.Brand.BrandId)
            .NotEmpty().WithMessage("BrandId zorunludur.")
            .Must(id => id != Guid.Empty).WithMessage("BrandId boş olamaz.");

        RuleFor(x => x.Brand.Name)
            .NotEmpty().WithMessage("Marka adı zorunludur.")
            .MinimumLength(2).WithMessage("Marka adı en az 2 karakter olmalıdır.")
            .MaximumLength(50).WithMessage("Marka adı en fazla 50 karakter olabilir.")
            .Matches(@"^[a-zA-ZçÇğĞıİöÖşŞüÜ\s]+$").WithMessage("Marka adı sadece harf ve boşluk içerebilir.")
            .Must(name => !string.IsNullOrWhiteSpace(name?.Trim())).WithMessage("Marka adı yalnızca boşluk olamaz.");
    }
}
