using CarRental.Application.DTOs.AppUser;
using FluentValidation;

namespace CarRental.Application.Validators.User
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(r => r.Name).NotEmpty().MinimumLength(3).MaximumLength(50).Matches(@"^[A-Za-z]*$");
            RuleFor(r => r.Surname).NotEmpty().MinimumLength(3).MaximumLength(50).Matches(@"^[A-Za-z]*$");
            RuleFor(r => r.Email).NotEmpty().MinimumLength(4).MaximumLength(256).Matches(@"^\w+([-.']\w+)*@\w+([-.]\w+)*$");
            RuleFor(r => r.Username).NotEmpty().MinimumLength(4).MaximumLength(256).Matches(@"^[A-Za-z0-9-._@+]*$");
            RuleFor(r => r.Password).NotEmpty().MinimumLength(8);
            RuleFor(r => r).Must(r => r.ConfirmPasswsord == r.Password);

        }
    }
}
