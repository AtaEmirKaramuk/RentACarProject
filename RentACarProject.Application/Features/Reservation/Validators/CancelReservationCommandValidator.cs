using FluentValidation;
using RentACarProject.Application.Features.Reservation.Commands;

public class CancelReservationCommandValidator : AbstractValidator<CancelReservationCommand>
{
    public CancelReservationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Rezervasyon ID boş olamaz.");
    }
}
