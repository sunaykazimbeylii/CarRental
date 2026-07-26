using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Application.DTOs.Token
{
    public record TokenResponseDto(string Token, string UserName, DateTime Expires);
}
