using AzerIsiq.Extensions.Enum;

namespace AzerIsiq.Dtos;

public class SubscriberFilterDto
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Patronymic { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FinCode { get; set; }
    public PopulationStatus? PopulationStatus { get; set; }
    public int? RegionId { get; set; }
    public string? RegionName { get; set; }
    public int? DistrictId { get; set; }
    public string? DistrictName { get; set; }
    public int? TerritoryId { get; set; }
    public string? TerritoryName { get; set; }
    public int? StreetId { get; set; }
    public string? StreetName { get; set; }
    public string? Building { get; set; }
    public string? Apartment { get; set; }
    public int? Status { get; set; }
    public string? Ats { get; set; }
    public string? SubscriberCode { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int Page { get; set; } 
    public int PageSize { get; set; } 
}