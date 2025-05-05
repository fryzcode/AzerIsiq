using AutoMapper;
using AzerIsiq.Dtos.ElectronicAppealDto;
using AzerIsiq.Models;

namespace AzerIsiq.Extensions.Mapping;

public class ElectronicAppealProfile : Profile
{
    public ElectronicAppealProfile()
    {
        CreateMap<ElectronicAppeal, ElectronicAppealDto>();
        CreateMap<ElectronicAppealCreateDto, ElectronicAppeal>();
    }
}