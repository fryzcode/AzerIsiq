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
        
        CreateMap<Subscriber, SubscriberProfileDto>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => $"{src.Name} {src.Surname} {src.Patronymic}"))
            .ForMember(dest => dest.PopulationStatus,
                opt => opt.MapFrom(src => src.PopulationStatus.ToString()))
            .ForMember(dest => dest.Region,
                opt => opt.MapFrom(src => src.Region!.Name))
            .ForMember(dest => dest.District,
                opt => opt.MapFrom(src => src.District!.Name))
            .ForMember(dest => dest.Territory,
                opt => opt.MapFrom(src => src.Territory != null ? src.Territory.Name : null))
            .ForMember(dest => dest.Street,
                opt => opt.MapFrom(src => src.Street != null ? src.Street.Name : null))
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => $"{src.Region!.Name}, {src.District!.Name}, {src.Street.Name ?? ""}, {src.Building}, {src.Apartment}"))
            .ForMember(dest => dest.RequestStatus,
                opt => opt.MapFrom(src => src.Status));
 
        CreateMap<Subscriber, SubscriberDebtDto>()
            .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.District != null ? src.District.Name : "Unknown"))
            .ForMember(dest => dest.TotalCurrentValue, opt => opt.MapFrom(src => 
                src.Counters.FirstOrDefault(c => c.SubscriberId == src.Id) != null 
                    ? src.Counters.First(c => c.SubscriberId == src.Id).CurrentValue 
                    : 0));

    }
}