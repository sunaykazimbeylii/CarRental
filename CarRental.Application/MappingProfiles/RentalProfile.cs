using AutoMapper;
using CarRental.Application.DTOs.Rental;
using CarRental.Domain.Entities;

namespace CarRental.Application.MappingProfiles;

public class RentalProfile : Profile
{
    public RentalProfile()
    {
        CreateMap<Rental, RentalGetDto>()
    .ForCtorParam(nameof(RentalGetDto.UserName),
        opt => opt.MapFrom(src => src.User.UserName))
    .ForCtorParam(nameof(RentalGetDto.CarModel),
        opt => opt.MapFrom(src => src.Car.Model));
    }
}
