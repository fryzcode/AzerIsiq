using AutoMapper;
using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Extensions.Mapping;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<SubscriberRequestDto, Subscriber>();
        CreateMap<Subscriber, SubscriberDto>();
    }
}