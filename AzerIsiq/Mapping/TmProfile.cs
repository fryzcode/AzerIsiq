using AutoMapper;
using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Extensions.Mapping;

public class TmProfile : Profile
{
    public TmProfile()
    {
        CreateMap<Tm, TmDto>().ReverseMap();

        CreateMap<Tm, TmGetDto>()
            .ForMember(dest => dest.Substation, opt => opt.MapFrom(src => src.Substation))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

        CreateMap<Substation, SubstationTmDto>()
            .ForMember(dest => dest.DistrictId, opt => opt.MapFrom(src => src.DistrictId))
            .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District));

        CreateMap<District, DistrictDto>();
        CreateMap<Region, RegionDto>();
        CreateMap<Location, LocationDto>();
        CreateMap<Image, ImageDto>();

        CreateMap<TmDto, Tm>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        CreateMap<Tm, TmDto>()
            .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.Substation.DistrictId))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Location.Address))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Location.Latitude.ToString("F6")))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Location.Longitude.ToString("F6")));
    }
}