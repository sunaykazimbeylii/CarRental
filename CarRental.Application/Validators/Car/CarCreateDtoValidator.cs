using CarRental.Application.DTOs.Car;
using FluentValidation;

namespace CarRental.Application.Validators.Car;

public class CarCreateDtoValidator : AbstractValidator<CarCreateDto>
{
    public CarCreateDtoValidator()
    {
        RuleFor(x => x.Model)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Year)
            .InclusiveBetween(1900, DateTime.Now.Year + 1);

        RuleFor(x => x.PlateNumber)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.DailyPrice)
            .GreaterThan(0);

        RuleFor(x => x.Mileage)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.BrandId)
            .GreaterThan(0);

        RuleFor(x => x.CategoryId)
            .GreaterThan(0);

        RuleFor(x => x.BranchId)
            .GreaterThan(0);

        RuleFor(x => x.ColorId)
            .GreaterThan(0);
    }
}