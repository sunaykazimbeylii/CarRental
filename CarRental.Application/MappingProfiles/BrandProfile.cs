using AutoMapper;
using CarRental.Application.DTOs.Brand;
using CarRental.Domain.Entities;

namespace CarRental.Application.MappingProfiles
{
    public class BrandProfile:Profile
    {
        public BrandProfile()
        {
            CreateMap<BrandCreateDto, Brand>();

            CreateMap<BrandUpdateDto, Brand>();

            CreateMap<Brand, BrandGetDto>();
        }

    }
}
