using AutoMapper;
using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Extensions.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserListDto>()
            .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.RoleName).ToList()));
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.UserRoles, opt =>
                opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.RoleName).ToList()))
            .ForMember(dest => dest.CreatedAt, opt =>
                opt.MapFrom(src => TimeZoneInfo.ConvertTimeToUtc(src.CreatedAt)));
    }
}