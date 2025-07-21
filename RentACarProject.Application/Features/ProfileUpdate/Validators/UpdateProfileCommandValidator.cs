using FluentValidation;
using RentACarProject.Application.DTOs.ProfileUpdate;

namespace RentACarProject.Application.Features.ProfileUpdate.Validators
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileDto>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(p => p.FirstName)
                .MaximumLength(20).WithMessage("İsim 20 karakterden uzun olamaz.")
                .Matches("^[a-zA-ZçÇğĞıİöÖşŞüÜ\\s]+$").WithMessage("İsim yalnızca harflerden oluşmalıdır.")
                .When(p => !string.IsNullOrWhiteSpace(p.FirstName));

            RuleFor(p => p.LastName)
                .MaximumLength(20).WithMessage("Soyisim 20 karakterden uzun olamaz.")
                .Matches("^[a-zA-ZçÇğĞıİöÖşŞüÜ\\s]+$").WithMessage("Soyisim yalnızca harflerden oluşmalıdır.")
                .When(p => !string.IsNullOrWhiteSpace(p.LastName));

            RuleFor(p => p.Email)
                .EmailAddress().WithMessage("Geçersiz email formatı.")
                .When(p => !string.IsNullOrWhiteSpace(p.Email));

            RuleFor(p => p.Phone)
                .Matches(@"^(\+90|0)[0-9]{10}$").WithMessage("Telefon numarası geçerli formatta olmalıdır. (Örn: +905xxxxxxxxx veya 05xxxxxxxxx)")
                .MaximumLength(13).WithMessage("Telefon numarası en fazla 13 karakter olabilir.")
                .When(p => !string.IsNullOrWhiteSpace(p.Phone));
        }
    }
}
