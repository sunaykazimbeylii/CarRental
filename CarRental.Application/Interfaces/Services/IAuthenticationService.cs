using CarRental.Application.DTOs.AppUser;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Application.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task RegisterAsync(RegisterDto userDto);
    }
}
