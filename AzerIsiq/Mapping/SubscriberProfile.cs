using AutoMapper;
using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Extensions.Mapping;

public class SubscriberProfile : Profile
{
    public SubscriberProfile()
    {
        CreateMap<SubscriberRequestDto, Subscriber>();
        CreateMap<Subscriber, SubscriberDto>();
        CreateMap<Subscriber, SubscriberDto>()
            .ForMember(dest => dest.RegionName, opt => opt.MapFrom(src => src.Region.Name ?? "N/A"))
            .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.District.Name ?? "N/A"))
            .ForMember(dest => dest.TerritoryName, opt => opt.MapFrom(src => src.Territory.Name ?? "N/A"))
            .ForMember(dest => dest.StreetName, opt => opt.MapFrom(src => src.Street.Name ?? "N/A"));
    }
}