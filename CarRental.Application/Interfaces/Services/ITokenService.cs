using CarRental.Application.DTOs.Token;
using CarRental.Domain.Entities;

namespace CarRental.Application.Interfaces.Services
{
    public interface ITokenService
    {
        TokenResponseDto CreateAccessToken(AppUser user, int minutes);
    }
}
