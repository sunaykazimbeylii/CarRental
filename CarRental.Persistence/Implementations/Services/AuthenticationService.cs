using AutoMapper;
using CarRental.Application.DTOs.AppUser;
using CarRental.Application.Interfaces.Services;
using CarRental.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace CarRental.Persistence.Implementations.Services
{
    internal class AuthenticationService: IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AuthenticationService(UserManager<AppUser> userManager,IMapper mapper)
        {
           _userManager = userManager;
           _mapper = mapper;
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
    }
}
