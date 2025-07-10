using FluentValidation;
using RentACarProject.Application.Features.Model.Commands;

namespace RentACarProject.Application.Features.Model.Validators
{
    public class UpdateModelCommandValidator : AbstractValidator<UpdateModelCommand>
    {
        public UpdateModelCommandValidator()
        {
            RuleFor(x => x.ModelId)
                .NotEmpty().WithMessage("ModelId zorunludur.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Model adı zorunludur.")
                .MaximumLength(50).WithMessage("Model adı en fazla 50 karakter olabilir.");

            RuleFor(x => x.BrandId)
                .NotEmpty().WithMessage("BrandId zorunludur.");
        }
    }
}
