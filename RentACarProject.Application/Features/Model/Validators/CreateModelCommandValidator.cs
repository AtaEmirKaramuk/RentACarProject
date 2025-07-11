using FluentValidation;

public class CreateModelCommandValidator : AbstractValidator<CreateModelCommand>
{
    public CreateModelCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Model adı zorunludur.")
            .MinimumLength(2).WithMessage("Model adı en az 2 karakter olmalıdır.")
            .MaximumLength(50).WithMessage("Model adı en fazla 50 karakter olabilir.")
            .Must(name => !string.IsNullOrWhiteSpace(name?.Trim())).WithMessage("Model adı yalnızca boşluk olamaz.");

        RuleFor(x => x.BrandId)
            .NotEmpty().WithMessage("BrandId zorunludur.")
            .Must(id => id != Guid.Empty).WithMessage("BrandId boş olamaz.");
    }
}
