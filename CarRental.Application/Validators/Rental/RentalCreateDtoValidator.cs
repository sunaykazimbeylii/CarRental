using CarRental.Application.DTOs.Rental;
using FluentValidation;

namespace CarRental.Application.Validators.Rental;

public class RentalCreateDtoValidator : AbstractValidator<RentalCreateDto>
{
    public RentalCreateDtoValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.CarId)
            .GreaterThan(0);

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Start date cannot be in the past.");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be later than start date.");

        RuleFor(x => x.PaymentMethod)
            .IsInEnum()
            .WithMessage("Invalid payment method.");
    }
}