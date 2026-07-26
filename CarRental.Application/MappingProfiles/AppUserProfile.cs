using AutoMapper;
using CarRental.Application.DTOs.AppUser;
using CarRental.Domain.Entities;

namespace CarRental.Application.MappingProfiles
{
    internal class AppUserProfile:Profile
    {
        public AppUserProfile()
        {
            CreateMap<RegisterDto, AppUser>();
        }
    }
}
