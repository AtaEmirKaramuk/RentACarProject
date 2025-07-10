using FluentValidation;
using RentACarProject.Application.Features.Auth.Commands;

namespace RentACarProject.Application.Validators.Auth
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Kullanıcı adı zorunludur.")
                .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır.")
                .MaximumLength(30).WithMessage("Kullanıcı adı en fazla 30 karakter olabilir.")
                .Matches(@"^[a-zA-Z0-9]+$").WithMessage("Kullanıcı adı sadece harf ve rakamlardan oluşmalıdır.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre zorunludur.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.,;:])[A-Za-z\d@$!%*?&.,;:]{6,}$")
                .WithMessage("Şifre en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email zorunludur.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad zorunludur.")
                .MaximumLength(20).WithMessage("Ad en fazla 20 karakter olabilir.")
                .Matches(@"^[a-zA-ZçÇğĞıİöÖşŞüÜ\s]+$").WithMessage("Ad sadece harflerden oluşmalıdır.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad zorunludur.")
                .MaximumLength(20).WithMessage("Soyad en fazla 20 karakter olabilir.")
                .Matches(@"^[a-zA-ZçÇğĞıİöÖşŞüÜ\s]+$").WithMessage("Soyad sadece harflerden oluşmalıdır.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon numarası zorunludur.")
                .Matches(@"^(\+90|0)[0-9]{10}$").WithMessage("Geçerli bir telefon numarası giriniz. (Örn: +905xxxxxxxxx veya 05xxxxxxxxx)")
                .MaximumLength(13).WithMessage("Telefon numarası en fazla 13 karakter olabilir.");
        }
    }
}
