using FluentValidation;
using RentACarProject.Application.Features.Reservation.Commands;

namespace RentACarProject.Application.Validators.Reservation
{
    public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
    {
        public CreateReservationCommandValidator()
        {
            RuleFor(x => x.Reservation.CarId)
                .NotEmpty().WithMessage("Araç seçilmelidir.");

            RuleFor(x => x.Reservation.PickupLocationId)
                .NotEmpty().WithMessage("Alış lokasyonu seçilmelidir.");

            RuleFor(x => x.Reservation.DropoffLocationId)
                .NotEmpty().WithMessage("Teslim lokasyonu seçilmelidir.");

            RuleFor(x => x.Reservation.StartDate)
                .NotEmpty().WithMessage("Başlangıç tarihi girilmelidir.");

            RuleFor(x => x.Reservation.EndDate)
                .NotEmpty().WithMessage("Bitiş tarihi girilmelidir.");

            RuleFor(x => x.Reservation)
                .Must(r => r.StartDate < r.EndDate)
                .WithMessage("Başlangıç tarihi, bitiş tarihinden önce olmalıdır.");
        }
    }
}
