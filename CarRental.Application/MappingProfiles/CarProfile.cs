using AutoMapper;
using CarRental.Application.DTOs.Car;
using CarRental.Domain.Entities;

namespace CarRental.Application.MappingProfiles
{
    public class CarProfile:Profile
    {
        public CarProfile()
        {
            CreateMap<CarCreateDto, Car>();

            CreateMap<CarUpdateDto, Car>();

            CreateMap<Car, CarGetDto>()
                .ForMember(dest => dest.BrandName,
                    opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.BranchName,
                    opt => opt.MapFrom(src => src.Branch.Name))
                .ForMember(dest => dest.ColorName,
                    opt => opt.MapFrom(src => src.Color.Name));
        }
    }
}
