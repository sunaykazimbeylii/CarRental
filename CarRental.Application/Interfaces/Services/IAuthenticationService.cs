using CarRental.Application.DTOs.AppUser;
using CarRental.Application.DTOs.Token;

namespace CarRental.Application.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task RegisterAsync(RegisterDto userDto);
        Task<TokenResponseDto> LoginAsync(LoginDto userDto);
    }
}
