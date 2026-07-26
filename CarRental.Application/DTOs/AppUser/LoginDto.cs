using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Application.DTOs.AppUser
{
    public record LoginDto(string UsernameOrEmail, string Password);
}
