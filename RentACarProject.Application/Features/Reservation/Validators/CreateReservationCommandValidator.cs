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
                .NotEmpty().WithMessage("Başlangıç tarihi boş olamaz.");

            RuleFor(x => x.Reservation.EndDate)
                .NotEmpty().WithMessage("Bitiş tarihi boş olamaz.");

            RuleFor(x => x.Reservation)
                .Must(x => x.StartDate < x.EndDate)
                .WithMessage("Başlangıç tarihi, bitiş tarihinden önce olmalıdır.");
        }
    }
}
