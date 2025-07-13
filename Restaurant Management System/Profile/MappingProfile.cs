using AutoMapper;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.DTOs;
namespace Restaurant_Management_System.Profile
{
    public class MappingProfile: AutoMapper.Profile

    {
        public MappingProfile()
        {
            CreateMap<MenuItem, MenuItemDto>()
    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
    .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
    .ReverseMap();
            CreateMap<MenuItemDto, MenuItem>()
    .ForMember(dest => dest.Category, opt => opt.Ignore());


        }
    }
}
