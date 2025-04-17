using AutoMapper;
using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Extensions.Mapping;

public class SubstationProfile : Profile
{
    public SubstationProfile()
    {
        CreateMap<Substation, SubstationDto>().ReverseMap();
        CreateMap<Substation, SubstationGetDto>()
            .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

        CreateMap<SubstationDto, Substation>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.LocationId, opt => opt.Ignore());

        CreateMap<District, DistrictDto>();
        CreateMap<Region, RegionDto>();
        CreateMap<Location, LocationDto>();
        CreateMap<Image, ImageDto>();

        CreateMap<SubstationDto, Substation>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Substation, SubstationResponseDto>();
        CreateMap<Substation, SubstationDto>()
            .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.District.RegionId))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Location.Address))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Location.Latitude.ToString("F6")))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Location.Longitude.ToString("F6")));
    }
}