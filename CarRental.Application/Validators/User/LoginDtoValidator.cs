using CarRental.Application.DTOs.AppUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Application.Validators.User
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(u => u.UsernameOrEmail).NotEmpty().MinimumLength(4).MaximumLength(256).Matches(@"^[A-Za-z0-9-._@+]*$");
            RuleFor(u => u.Password).NotEmpty().MinimumLength(8);
        }
    }
}
