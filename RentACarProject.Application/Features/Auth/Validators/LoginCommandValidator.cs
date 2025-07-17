using FluentValidation;
using RentACarProject.Application.Features.Auth.Commands;

namespace RentACarProject.Application.Features.Auth.Validators
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz.")
                .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır.")
                .MaximumLength(30).WithMessage("Kullanıcı adı en fazla 20 karakter olabilir.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz.")
                .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
                .MaximumLength(20).WithMessage("Şifre en fazla 20 karakter olabilir.")
                .Matches(@"[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.")
                .Matches(@"[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir.")
                .Matches(@"\d").WithMessage("Şifre en az bir rakam içermelidir.");
        }
    }
}
