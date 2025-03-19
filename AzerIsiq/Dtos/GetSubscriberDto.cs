using AzerIsiq.Extensions.Enum;

namespace AzerIsiq.Dtos;

public class GetSubscriberDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Patronymic { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string FinCode { get; set; } = null!;
    public PopulationStatus PopulationStatus { get; set; }
    public int RegionId { get; set; }
    public string RegionName { get; set; } = null!;
    public int DistrictId { get; set; }
    public string DistrictName { get; set; } = null!;
    public int TerritoryId { get; set; }
    public string TerritoryName { get; set; } = null!;
    public int StreetId { get; set; }
    public string StreetName { get; set; } = null!;
    public string Building { get; set; } = null!;
    public string Apartment { get; set; } = null!;
    public int Status { get; set; }
    public string? Ats { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? SubscriberCode { get; set; }
}
