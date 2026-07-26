using AutoMapper;
using CarRental.Application.DTOs.AppUser;
using CarRental.Application.DTOs.Token;
using CarRental.Application.Interfaces.Services;
using CarRental.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CarRental.Persistence.Implementations.Services
{
    internal class AuthenticationService: IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AuthenticationService(
            UserManager<AppUser> userManager,
            IMapper mapper,
            ITokenService tokenService
            )
        {
           _userManager = userManager;
           _mapper = mapper;
            _tokenService = tokenService;
        }
         public async  Task RegisterAsync(RegisterDto userDto)
        {
            IdentityResult result = await _userManager.CreateAsync(_mapper.Map<AppUser>(userDto), userDto.Password);
            if (!result.Succeeded)
            {
                StringBuilder sb = new();
                foreach (IdentityError error  in result.Errors)
                {
                    sb.Append(error.Description);
                }
                throw new Exception(sb.ToString());

            }
        }
        public async Task<TokenResponseDto> LoginAsync(LoginDto userDto)
        {
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userDto.UsernameOrEmail || u.Email == userDto.UsernameOrEmail);
            if (user is null)
            {
                throw new Exception("UserName or Password  is invalid");
            }
            bool result = await _userManager.CheckPasswordAsync(user, userDto.Password);
            if (!result)
            {
                user.AccessFailedCount++;
                throw new Exception("UserName or Password  is invalid");

            }

            return _tokenService.CreateAccessToken(user, 15);

        }
    }
}
